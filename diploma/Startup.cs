using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(diploma.Startup))]
namespace diploma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
