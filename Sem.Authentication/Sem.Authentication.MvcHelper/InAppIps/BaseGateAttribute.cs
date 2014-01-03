// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FastRequestsProtectionAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the FastRequestsProtectionAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.InAppIps.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute protects against multiple fast requests from a single client.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class BaseGateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The context processors that do host the id extractors and the client statistic.
        /// </summary>
        private readonly IEnumerable<ContextProcessor> contextProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastRequestsProtectionAttribute"/> class.
        /// </summary>
        protected BaseGateAttribute()
            : this(typeof(SessionIdExtractor), typeof(UserHostExtractor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastRequestsProtectionAttribute"/> class.
        /// </summary>
        /// <param name="extractors">The types of extractors to use.</param>
        protected BaseGateAttribute(params Type[] extractors)
        {
            this.contextProcessors = 
                extractors
                    .Select(x => x.GetConstructor(new Type[] { }))
                    .Where(x => x != null)
                    .Select(x => new ContextProcessor((IIdExtractor)x.Invoke(null)));
        }

        /// <summary>
        /// Gets or sets the action to redirect to in case of a fault. 
        /// The action does contain a string parameter <c>FaultSource</c> with the name of this class.
        /// </summary>
        public string FaultAction { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Where we collect come information about the client request and update the statistics. We also will prevent further request processing by throwing exceptions
        /// if the statists do tell us that this client is an attacker.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context != null)
            {
                if (this.contextProcessors.Any(processor => !this.StatisticsGate(processor.IdExtractor.Extract(context), processor.Statistics)))
                {
                    if (string.IsNullOrEmpty(this.FaultAction))
                    {
                        filterContext.Result = new ContentResult
                            {
                                Content = "client has been blocked...",
                            };

                        // see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                        return;
                    }

                    var controller = (System.Web.Mvc.Controller)filterContext.Controller;
                    var action = controller.Url.Action(this.FaultAction, new { FaultSource = this.GetType().Name });
                    filterContext.Result = new RedirectResult(action);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// The statistics gate does check the clients statistics and prohibits further processing of the request if the client
        /// requests this resource too often.
        /// </summary>
        /// <param name="clientId"> The client ID may be a session id or a client IP.  </param>
        /// <param name="statistics"> The statistics collection that match the type of the client id.  </param>
        /// <returns> A value indicating whether the client is allowed to go on. </returns>
        protected abstract bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics);
    }
}