using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Laptopp.Startup))]
namespace Laptopp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
