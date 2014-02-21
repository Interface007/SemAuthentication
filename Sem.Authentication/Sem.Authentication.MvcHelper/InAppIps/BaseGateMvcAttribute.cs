// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseGateMvcAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the FastRequestsProtectionAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Sem.Authentication.AppInfrastructure;
    using Sem.Authentication.InAppIps.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute protects against multiple fast requests from a single client.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class BaseGateMvcAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGateMvcAttribute"/> class.
        /// </summary>
        protected BaseGateMvcAttribute()
            : this(typeof(SessionIdExtractor), typeof(UserHostExtractor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGateMvcAttribute"/> class.
        /// </summary>
        /// <param name="extractors">The types of extractors to use.</param>
        protected BaseGateMvcAttribute(params Type[] extractors)
        {
            this.ContextProcessors =
                extractors.Select(x => x.GetConstructor(new Type[] { }))
                    .Where(x => x != null)
                    .Select(x => new ContextProcessor((IIdExtractor)x.Invoke(null)));
        }

        /// <summary>
        /// Gets the context processors set in the constructor.
        /// </summary>
        public IEnumerable<ContextProcessor> ContextProcessors { get; private set; }
        
        /// <summary>
        /// Gets the instance that implements the functionality of this gate.
        /// </summary>
        public abstract IGate Instance { get; }

        /// <summary>
        /// Gets or sets the action to be redirected to in case of a fault.
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
            filterContext.ArgumentMustNotBeNull("filterContext");
            var context = filterContext.HttpContext;
            if (context != null)
            {
                var instance = this.Instance;
                var checkStatisticGate = instance.CheckStatisticGate;   // for performance reason only read this once
                var checkRequestGate = instance.CheckRequestGate;       // for performance reason only read this once

                var httpRequestBase = context.Request;
                var gateClosed = this.ContextProcessors.Any(processor =>
                {
                    var clientId = processor.IdExtractor.Extract(context);
                    return (checkStatisticGate && !instance.StatisticsGate(clientId, processor.Statistics))
                        || (checkRequestGate && !instance.RequestGate(clientId, httpRequestBase));
                });

                if (gateClosed)
                {
                    if (string.IsNullOrEmpty(this.FaultAction))
                    {
                        filterContext.Result = new ContentResult
                        {
                            Content = "client has been blocked...",
                        };

                        // see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                        var response = filterContext.HttpContext.Response;
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        response.TrySkipIisCustomErrors = true;
                        return;
                    }

                    var controller = (System.Web.Mvc.Controller)filterContext.Controller;
                    var action = controller.Url.Action(this.FaultAction, new { FaultSource = this.GetType().Name });
                    filterContext.Result = new RedirectResult(action);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}