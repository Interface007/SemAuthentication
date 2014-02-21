// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FastRequestsProtection.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Decorates any MVC route that needs to have client requests limited by time.
//   This attribute protects against multiple fast requests from a single client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;

    using Sem.Authentication.AppInfrastructure;
    using Sem.Authentication.MvcHelper.AppInfrastructure;
    using Sem.Authentication.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute protects against multiple fast requests from a single client.
    /// </summary>
    public sealed class FastRequestsProtection : BaseGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FastRequestsProtection"/> class.
        /// </summary>
        public FastRequestsProtection()
        {
            this.MaxRetentionTimeOfStatistics = 3000;
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
        /// The statistics gate does check the clients statistics and prohibits further processing of the request if the client
        /// requests this resource too often.
        /// </summary>
        /// <param name="clientId"> The client ID may be a session id or a client IP.  </param>
        /// <param name="statistics"> The statistics collection that match the type of the client id.  </param>
        /// <returns> A value indicating whether the client is allowed to go on. </returns>
        public override bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            // no client id - nothing to do...
            if (string.IsNullOrEmpty(clientId))
            {
                return true;
            }

            statistics.ArgumentMustNotBeNull("statistics");

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