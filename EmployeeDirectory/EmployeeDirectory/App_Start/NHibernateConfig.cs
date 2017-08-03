using System;
using System.Configuration;
using DataAccess.Core;
using DataAccess.Core.Conventions;
using Employee.DomainObject.Mappings;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.Practices.Unity;
using NHibernate;

namespace EmployeeDirectory
{
    public class NHibernateConfig
    {
        private static ISessionFactory CreateSessionFactory()
        {
            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    .ExposeConfiguration
                (
                    c => c.SetProperty("current_session_context_class", "web")
                ); ;

            var configuration = config.BuildConfiguration();
            var model = AutoMap.AssemblyOf<EmployeeMapping>()
                .Where(t => t.Namespace == typeof(EmployeeMapping).Namespace ||
                            t.Namespace == typeof(Employee.DomainObject.Employee).Namespace)
                .Conventions.AddFromAssemblyOf<ForeignKeyNameConvention>();

            configuration.AddAutoMappings(model);
            return configuration.BuildSessionFactory();
        }

        public static void RegistorSessionFactory()
        {
            var container = UnityConfig.Current;
            if(container == null) 
                throw new  Exception("Container must be set up before nhibernate setup");
            var sessionFactory = CreateSessionFactory();
            container.RegisterInstance(sessionFactory, new ContainerControlledLifetimeManager());
            container.RegisterType<IRepository, Repository>();
            container.RegisterType<ISessionManager, SessionManager>(new PerResolveLifetimeManager());
        }
    }
}