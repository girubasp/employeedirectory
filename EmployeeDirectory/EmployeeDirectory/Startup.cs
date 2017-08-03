using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeeDirectory.Startup))]
namespace EmployeeDirectory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
