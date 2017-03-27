using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PP_demo.Startup))]
namespace PP_demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
