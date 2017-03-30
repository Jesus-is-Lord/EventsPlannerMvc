using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EventsPlannerMvc.Startup))]
namespace EventsPlannerMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
