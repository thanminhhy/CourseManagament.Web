using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CourseManagament.Web.Startup))]
namespace CourseManagament.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
