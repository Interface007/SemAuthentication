// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseGateAttributeWebApi.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the FastRequestsProtectionAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.WebApiHelper.InAppIps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Sem.Authentication.AppInfrastructure;
    using Sem.Authentication.InAppIps.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute protects against multiple fast requests from a single client.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class BaseGateWebApiAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGateWebApiAttribute"/> class.
        /// </summary>
        protected BaseGateWebApiAttribute()
            : this(typeof(SessionIdExtractor), typeof(UserHostExtractor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGateWebApiAttribute"/> class.
        /// </summary>
        /// <param name="extractors">The types of extractors to use.</param>
        protected BaseGateWebApiAttribute(params Type[] extractors)
        {
            this.ContextProcessors =
                extractors.Select(x => x.GetConstructor(new Type[] { }))
                    .Where(x => x != null)
                    .Select(x => new ContextProcessor((IIdExtractor)x.Invoke(null)));
        }

        /// <summary>
        /// Gets the context processors are the instances using the client ID extractors.
        /// </summary>
        public IEnumerable<ContextProcessor> ContextProcessors { get; private set; }

        /// <summary>
        /// Gets the instance that implements the functionality of this gate.
        /// </summary>
        public abstract IGate Instance { get; }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.ArgumentMustNotBeNull("actionContext");
            if (actionContext != null)
            {
                var checkStatisticGate = this.Instance.CheckStatisticGate;   // for performance reason only read this once
                var checkRequestGate = this.Instance.CheckRequestGate;       // for performance reason only read this once

                var httpContextBase = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
                var requestBase = httpContextBase == null ? null : httpContextBase.Request;
                var gateClosed = this.ContextProcessors.Any(processor =>
                {
                    var clientId = processor.IdExtractor.Extract(httpContextBase);
                    return (checkStatisticGate && !this.Instance.StatisticsGate(clientId, processor.Statistics))
                        || (checkRequestGate && !this.Instance.RequestGate(clientId, requestBase));
                });

                if (gateClosed)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}