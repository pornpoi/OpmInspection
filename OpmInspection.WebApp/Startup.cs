using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OpmInspection.WebApp.Startup))]
namespace OpmInspection.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
