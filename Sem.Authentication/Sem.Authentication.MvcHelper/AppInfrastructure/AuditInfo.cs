// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditInfo.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the AuditInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System.Linq;
    using System.Web.Routing;

    public class AuditInfo<T>
    {
        public AuditInfo(RequestContext requestContext, T exception)
            : this(requestContext)
        {
            this.Details = exception;
        }

        public AuditInfo(RequestContext requestContext)
        {
            requestContext
                .ArgumentMustNotBeNull("requestContext")
                .ArgumentPropertyMustNotBeNull("requestContext", "RouteData", x => x.RouteData)
                .ArgumentPropertyMustNotBeNull("requestContext", "RouteData.Values", x => x.RouteData.Values)
                .ArgumentPropertyMustNotBeNull("requestContext", "HttpContext", x => x.HttpContext)
                .ArgumentPropertyMustNotBeNull("requestContext", "HttpContext.User", x => x.HttpContext.User);

            this.Action = requestContext.RouteData.Values.Aggregate(string.Empty, (c, x) => c + "=>" + x.Value);
            this.User = requestContext.HttpContext.User.Identity.Name;
        }

        public AuditInfo(string user, string action)
        {
            this.User = user;
            this.Action = action;
        }

        public string User { get; set; }
        public string Action { get; set; }
        public T Details { get; set; }
    }
}