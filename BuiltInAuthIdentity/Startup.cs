using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuiltInAuthIdentity.Startup))]
namespace BuiltInAuthIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
