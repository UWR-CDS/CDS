using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CDSWebsite.Startup))]
namespace CDSWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
