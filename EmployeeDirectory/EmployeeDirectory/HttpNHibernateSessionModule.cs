using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Core;
using Microsoft.Practices.Unity;

namespace EmployeeDirectory
{
    public class HttpNHibernateSessionModule : IHttpModule
    {
        private const string SessionManagerKey = "sessionManager";
        private const string ErrorOccuried = "ErrorOccured";

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
            context.Error += context_Error;
        }

        private void context_Error(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;
            context.Items.Add(ErrorOccuried, true);
        }

        public void Dispose()
        {
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;


            var container = UnityConfig.Current;

            if (container.IsRegistered<ISessionManager>())
            {
                var sessionManager = container.Resolve<ISessionManager>();

                // Unit of Work start WILL start all Sessions!!!
                sessionManager.Start();

                context.Items.Add(SessionManagerKey, sessionManager);
            }

        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;


            var container = UnityConfig.Current;

            if (container.IsRegistered<ISessionManager>())
            {
                var sessionManager = (ISessionManager)(context.Items[SessionManagerKey]);
                var successfulRequest = !context.Items.Contains(ErrorOccuried);

                try
                {
                    if (successfulRequest)
                        sessionManager.Complete();
                }
                finally
                {
                    sessionManager.End();
                }
            }

        }
    }
}