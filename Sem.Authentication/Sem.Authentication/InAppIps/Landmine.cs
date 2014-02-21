// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Landmine.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The landmine attribute does cache the value of a web landmine.
//   The basic idea is to embed an http field that looks as an easy target for hackers. As soon as the
//   content of the field is changed, the user will be locked out.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps
{
    using System;
    using System.Web;
    using System.Web.Caching;

    using Sem.Authentication.AppInfrastructure;

    /// <summary>
    /// The landmine attribute does cache the value of a web landmine.
    /// The basic idea is to embed an http field that looks as an easy target for hackers. As soon as the 
    /// content of the field is changed, the user will be locked out.
    /// </summary>
    public class Landmine : BaseGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Landmine"/> class.
        /// </summary>
        public Landmine()
        {
            this.LandmineName = "Landmine";
            this.ExpectedValue = "8008";
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
        /// Gets or sets the expected value for the land mine.
        /// </summary>
        public string ExpectedValue { get; set; }

        /// <summary>
        /// Gets or sets the request area where to search for the landmine value.
        /// </summary>
        public RequestArea RequestArea { get; set; }

        public bool AllowErrorInStatusText { get; set; }

        /// <summary>
        /// The request gate does check the content of the request to check whether the 
        /// land mine is still intact or if it has been triggered by changing the content 
        /// into an unexpected value.
        /// </summary>
        /// <param name="clientId"> The client id. </param>
        /// <param name="request"> The request. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public override bool RequestGate(string clientId, HttpRequestBase request)
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

            var currentValue =
                this.RequestArea == RequestArea.Form ? request.Form[this.LandmineName] :
                this.RequestArea == RequestArea.Header ? request.Headers[this.LandmineName] :
                this.RequestArea == RequestArea.QueryString ? request.QueryString[this.LandmineName] :
                string.Empty;

            if (currentValue == this.ExpectedValue)
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