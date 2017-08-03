using System.Web.Mvc;
using Employee.Common;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Employee.Service;
using UserManagement.Service;

namespace EmployeeDirectory
{
    public static class UnityConfig
    {
        public static IUnityContainer Current { get; private set; }
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<ISearchEmployeeService, SearchEmployeeService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType(typeof(ILogger<>), typeof(Logger<>));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            Current = container;
        }
    }
}