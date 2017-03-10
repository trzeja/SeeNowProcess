using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SeeNowProcess.Startup))]
namespace SeeNowProcess
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
