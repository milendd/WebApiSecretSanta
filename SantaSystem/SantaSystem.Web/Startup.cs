using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SantaSystem.Web.Startup))]

namespace SantaSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
