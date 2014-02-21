// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinimumRequestTimeDistance.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Decorates any MVC route that needs to have client requests limited by time.
//   This attribute does enforce a minimum time between two requests from the same IP.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps
{
    using System;
    using System.Collections.Concurrent;
    using System.Web;
    using System.Web.Caching;

    using Sem.Authentication.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute does enforce a minimum time between two requests from the same IP.
    /// </summary>
    public sealed class MinimumRequestTimeDistance : BaseGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumRequestTimeDistance"/> class with 
        /// "1 second" between each request of one client.
        /// </summary>
        public MinimumRequestTimeDistance()
        {
            this.Seconds = 1;
        }

        /// <summary>
        /// Gets or sets a unique name for this throttle.
        /// </summary>
        /// <remarks> We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1" </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or sets a text message that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The statistics gate does check the whether the extracted client id did execute a request
        /// in the past x seconds.
        /// </summary>
        /// <param name="clientId"> The client id. </param>
        /// <param name="statistics"> The statistics dictionary. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public override bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            // no client id - nothing to do...
            if (string.IsNullOrEmpty(clientId))
            {
                return true;
            }

            var cache = HttpRuntime.Cache;
            var key = "LastRequestKey-" + clientId;
            if (cache[key] != null)
            {
                return false;
            }
            
            cache.Add(
                key,
                true,                                   // is this the smallest data we can have?
                null,                                   // no dependencies
                DateTime.Now.AddSeconds(this.Seconds),  // absolute expiration
                Cache.NoSlidingExpiration,
                CacheItemPriority.Low,
                null);                                  // no callback

            return true;
        }
    }
}