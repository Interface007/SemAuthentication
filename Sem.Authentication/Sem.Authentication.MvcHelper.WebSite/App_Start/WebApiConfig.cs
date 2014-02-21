namespace Sem.Authentication.MvcHelper.WebSite.App_Start
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}