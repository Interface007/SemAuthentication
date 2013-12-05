using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sem.Authentication.MvcHelper.WebSite.Startup))]
namespace Sem.Authentication.MvcHelper.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
