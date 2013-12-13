// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientStatistic.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClientStatistic type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The statistic about requests done by a single client.
    /// </summary>
    public class ClientStatistic
    {
        /// <summary>
        /// The number of requests since the creation of the statistic.
        /// </summary>
        private int requests;

        /// <summary>
        /// The object ID - just a unique ID for this object instance.
        /// </summary>
        private string id;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStatistic"/> class.
        /// </summary>
        public ClientStatistic()
        {
            this.FirstRequest = DateTime.UtcNow;
            this.LastRequest = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the date and time of the first request, this item represents.
        /// </summary>
        public DateTime FirstRequest { get; private set; }

        /// <summary>
        /// Gets the date and time of the last request.
        /// </summary>
        public DateTime LastRequest { get; private set; }

        /// <summary>
        /// Gets the requests per second.
        /// </summary>
        public int RequestsPerSecond
        {
            get
            {
                var milliseconds = (this.LastRequest - this.FirstRequest).TotalMilliseconds;
                Debug.Print("Milliseconds: {0}; Requests: {1}", milliseconds, this.requests);
                return milliseconds > 1000 ? (int)((this.requests * 1000) / milliseconds) : this.requests;
            }
        }

        /// <summary>
        /// Gets the id of this object instance.
        /// It's a heisenberg-id ... it will be generated the first time you try to access it ;-).
        /// </summary>
        public string Id
        {
            get
            {
                return this.id ?? (this.id = Guid.NewGuid().ToString("N"));
            }
        }

        /// <summary>
        /// Increases the request count.
        /// </summary>
        public void IncreaseRequestCount()
        {
            this.LastRequest = DateTime.UtcNow;
            this.requests += 1;
        }
    }
}