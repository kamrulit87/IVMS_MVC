using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IVMS.Startup))]
namespace IVMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
