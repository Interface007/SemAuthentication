// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LandmineAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the LandmineAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System;
    using System.Web;
    using System.Web.Caching;

    using Sem.Authentication.MvcHelper.AppInfrastructure;
    using Sem.Authentication.MvcHelper.InAppIps.Processing;

    /// <summary>
    /// The landmine attribute does cache the value of a web landmine.
    /// The basic idea is to embed an http field that looks as an easy target for hackers. As soon as the 
    /// content of the field is changed, the user will be locked out.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LandmineAttribute : BaseGateAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LandmineAttribute"/> class.
        /// </summary>
        public LandmineAttribute()
            : this(typeof(SessionIdExtractor), typeof(UserHostExtractor))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandmineAttribute"/> class.
        /// </summary>
        /// <param name="extractors">The types of extractors to use.</param>
        public LandmineAttribute(params Type[] extractors)
            : base(extractors)
        {
            this.LandmineName = "Landmine";
            this.Seconds = 120;
        }

        /// <summary>
        /// Gets or sets the number of seconds clients must wait before the user will be 
        /// able to call this method again after triggering the land mine.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or sets the name of the land mine this attribute belongs to.
        /// </summary>
        public string LandmineName { get; set; }

        /// <summary>
        /// The request gate does check the content of the request to check whether the 
        /// land mine is still intact or if it has been triggered by changing the content 
        /// into an unexpected value.
        /// </summary>
        /// <param name="clientId"> The client id. </param>
        /// <param name="request"> The request. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        protected override bool RequestGate(string clientId, HttpRequestBase request)
        {
            // no client id - nothing to do...
            if (string.IsNullOrEmpty(clientId))
            {
                return true;
            }

            request.ArgumentMustNotBeNull("request");

            var cache = HttpRuntime.Cache;
            var key = "Landmined-" + clientId;
            if (cache[key] != null)
            {
                return false;
            }

            if (request.Form[this.LandmineName] == "8008")
            {
                return true;
            }

            cache.Add(
                key,
                true,                                   // is this the smallest data we can have?
                null,                                   // no dependencies
                DateTime.Now.AddSeconds(this.Seconds),  // absolute expiration
                Cache.NoSlidingExpiration,
                CacheItemPriority.Low,
                null);                                  // no callback

            return false;
        }
    }
}