// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuditInfo.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the AuditInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using System.Linq;
    using System.Web.Routing;

    /// <summary>
    /// The audit log information structure.
    /// </summary>
    /// <typeparam name="T"> The type of detail data. </typeparam>
    public class AuditInfo<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditInfo{T}"/> class.
        /// </summary>
        /// <param name="requestContext"> The request context to extract user and action information. </param>
        /// <param name="exception"> The exception to log. </param>
        public AuditInfo(RequestContext requestContext, T exception)
            : this(requestContext)
        {
            this.Details = exception;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditInfo{T}"/> class.
        /// </summary>
        /// <param name="requestContext"> The request context to extract user and action information. </param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditInfo{T}"/> class.
        /// </summary>
        /// <param name="user"> The user that did execute the action. </param>
        /// <param name="action"> The action that has been executed. </param>
        public AuditInfo(string user, string action)
        {
            this.User = user;
            this.Action = action;
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the detail data about the action execution (e.g. the exception).
        /// </summary>
        public T Details { get; set; }
    }
}