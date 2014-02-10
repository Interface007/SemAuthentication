// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseGateAttribute.cs" company="Sven Erik Matzen">
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
    using System.Web;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.AppInfrastructure;
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
        /// Initializes a new instance of the <see cref="BaseGateAttribute"/> class. 
        /// </summary>
        protected BaseGateAttribute()
            : this(typeof(SessionIdExtractor), typeof(UserHostExtractor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGateAttribute"/> class. 
        /// </summary>
        /// <param name="extractors"> The types of extractors to use. </param>
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
        /// Gets a value indicating whether to check the statistic gate. If this property 
        /// is overridden, it can control whether to check the statistics of a request using the method
        /// <see cref="StatisticsGate"/>. If you override this property you can return "false" in order to 
        /// prevent time consuming processing that only applies to calling <see cref="StatisticsGate"/>.
        /// </summary>
        public virtual bool CheckStatisticGate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to check the request gate. If this property 
        /// is overridden, it can control whether to check the request of a request using the method
        /// <see cref="RequestGate"/>. If you override this property you can return "false" in order to 
        /// prevent time consuming processing that only applies to calling <see cref="RequestGate"/>.
        /// </summary>
        public virtual bool CheckRequestGate
        {
            get
            {
                return true;
            }
        }

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
                var checkStatisticGate = this.CheckStatisticGate;   // for performance reason only read this once
                var checkRequestGate = this.CheckRequestGate;       // for performance reason only read this once

                var httpRequestBase = context.Request;
                var gateClosed = this.contextProcessors.Any(processor =>
                    {
                        var clientId = processor.IdExtractor.Extract(context);
                        return (checkStatisticGate && !this.StatisticsGate(clientId, processor.Statistics))
                            || (checkRequestGate && !this.RequestGate(clientId, httpRequestBase));
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
        /// requests this resource too often. You should set <see cref="CheckStatisticGate"/> to false inside your attribute 
        /// implementation if it does not override this method.
        /// </summary>
        /// <param name="clientId"> The client ID may be a session id or a client IP.  </param>
        /// <param name="statistics"> The statistics collection that match the type of the client id.  </param>
        /// <returns> A value indicating whether the client is allowed to go on. </returns>
        protected virtual bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            return true;
        }

        /// <summary>
        /// The request gate does check the content or meta data of the request. You should set <see cref="CheckRequestGate"/>
        /// to false inside your attribute implementation if it does not override this method.
        /// </summary>
        /// <param name="clientId"> The client id. </param>
        /// <param name="request"> The request. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        protected virtual bool RequestGate(string clientId, HttpRequestBase request)
        {
            return true;
        }
    }
}