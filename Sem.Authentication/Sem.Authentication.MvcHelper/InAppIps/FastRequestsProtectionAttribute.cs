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
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The fast requests protection attribute.
    /// </summary>
    public class FastRequestsProtectionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The context processors that do host the id extractors and the client statistic.
        /// </summary>
        private readonly IEnumerable<ContextProcessor> contextProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastRequestsProtectionAttribute"/> class.
        /// </summary>
        public FastRequestsProtectionAttribute()
            : this(typeof(SessionIdExtractor), typeof(UserHostExtractor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastRequestsProtectionAttribute"/> class.
        /// </summary>
        /// <param name="extractors">The types of extractors to use.</param>
        public FastRequestsProtectionAttribute(params Type[] extractors)
        {
            this.MaxRetentionTimeOfStatistics = 3000;
            this.contextProcessors = 
                extractors
                    .Select(x => x.GetConstructor(new Type[] { }))
                    .Where(x => x != null)
                    .Select(x => new ContextProcessor((IIdExtractor)x.Invoke(null)));
        }

        /// <summary>
        /// Gets or sets the maximum retention time in milliseconds of statistics - when a client does not issue requests in this period of time, we will remove the statistic.
        /// </summary>
        public int MaxRetentionTimeOfStatistics { get; set; }

        /// <summary>
        /// Gets or sets the requests per second the client is allowed to do.
        /// </summary>
        public int RequestsPerSecondAndClient { get; set; }

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
            var context = filterContext.RequestContext.HttpContext;
            if (context != null)
            {
                if (this.contextProcessors.Any(processor => !this.StatisticsGate(processor.IdExtractor.Extract(context), processor.Statistics)))
                {
                    if (string.IsNullOrEmpty(this.FaultAction))
                    {
                        throw new HttpException(403, "Request denied for this client.");
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
        private bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            // no client id - nothing to do...
            if (string.IsNullOrEmpty(clientId))
            {
                return true;
            }

            // cleanup old statistics (we will not take care for clients that are slower than 1 request per X millisec.)
            var time = DateTime.UtcNow.AddMilliseconds(-this.MaxRetentionTimeOfStatistics);
            foreach (var statistic in statistics.Where(x => x.Value.LastRequest < time).ToArray())
            {
                ClientStatistic value;
                statistics.TryRemove(statistic.Key, out value);
            }

            var clientStatistic = statistics.GetOrAdd(clientId, new ClientStatistic());
            clientStatistic.IncreaseRequestCount();

            var requestsPerSecond = clientStatistic.RequestsPerSecond;
            Debug.Print("client statistic for ID {0}: {1} requests per second ({2}|{3}) {4}", clientId, requestsPerSecond, clientStatistic.FirstRequest, clientStatistic.LastRequest, clientStatistic.Id);
            return requestsPerSecond <= this.RequestsPerSecondAndClient;
        }
    }
}