// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinimumRequestTimeDistanceAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the MinimumRequestTimeDistanceAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.InAppIps.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute does enforce a minimum time between two requests from the same IP.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MinimumRequestTimeDistanceAttribute : BaseGateAttribute
    {
        public MinimumRequestTimeDistanceAttribute()
        {
            this.Seconds = 1;
        }

        /// <summary>
        /// A unique name for this Throttle.
        /// </summary>
        /// <remarks> We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1" </remarks>
        public string Name { get; set; }

        /// <summary>
        /// The number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// A text message that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
        /// </summary>
        public string Message { get; set; }

        protected override bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            // no client id - nothing to do...
            if (string.IsNullOrEmpty(clientId))
            {
                return true;
            }

            var cache = HttpRuntime.Cache;
            var key = "LastRequestKeay-" + clientId;
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
