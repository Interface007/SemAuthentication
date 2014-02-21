// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the Startup type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
