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
        /// The session statistics.
        /// </summary>
        private readonly ConcurrentDictionary<string, ClientStatistic> sessionStatistics = new ConcurrentDictionary<string, ClientStatistic>();

        /// <summary>
        /// The user host statistics.
        /// </summary>
        private readonly ConcurrentDictionary<string, ClientStatistic> userHostStatistics = new ConcurrentDictionary<string, ClientStatistic>();

        /// <summary>
        /// The max retention time of statistics - when a client does not issue requests in this period of time, we will remove the statistic.
        /// </summary>
        private readonly int maxRetentionTimeOfStatistics;

        /// <summary>
        /// Initializes a new instance of the <see cref="FastRequestsProtectionAttribute"/> class.
        /// </summary>
        public FastRequestsProtectionAttribute()
        {
            this.maxRetentionTimeOfStatistics = 30;
        }

        /// <summary>
        /// Gets or sets the requests per second the client is allowed to do.
        /// </summary>
        public int RequestsPerSecondAndClient { get; set; }

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
                var session = context.Session;
                if (session != null)
                {
                    this.StatisticsGate(session.SessionID, this.sessionStatistics);
                }

                var request = context.Request;
                if (request != null)
                {
                    this.StatisticsGate(request.UserHostAddress, this.userHostStatistics);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// The statistics gate does check the clients statistics and prohibits further processing of the request if the client
        /// requests this resource too often.
        /// </summary>
        /// <param name="clientId"> The client ID may be a session id or a client IP. </param>
        /// <param name="statistics"> The statistics collection that match the type of the client id. </param>
        private void StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            // todo: make it configurable
            // cleanup old statistics (we will not take care for clients that are slower than 1 request per 30 sec.)
            foreach (var statistic in statistics.Where(x => x.Value.LastRequest < DateTime.UtcNow.AddSeconds(-this.maxRetentionTimeOfStatistics)).ToArray())
            {
                ClientStatistic value;
                statistics.TryRemove(statistic.Key, out value);
            }

            var clientStatistic = statistics.GetOrAdd(clientId, new ClientStatistic());
            clientStatistic.IncreaseRequestCount();
            Debug.Print("Client ID: {0}, Request Count: {1}", clientId, clientStatistic.RequestsPerSecond);
            if (clientStatistic.RequestsPerSecond > this.RequestsPerSecondAndClient)
            {
                throw new HttpException(403, "Request denied for this client.");
            }
        }
    }
}