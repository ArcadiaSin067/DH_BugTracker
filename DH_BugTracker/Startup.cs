using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DH_BugTracker.Startup))]
namespace DH_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
