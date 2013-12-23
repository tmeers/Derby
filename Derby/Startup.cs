using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Derby.Startup))]
namespace Derby
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
