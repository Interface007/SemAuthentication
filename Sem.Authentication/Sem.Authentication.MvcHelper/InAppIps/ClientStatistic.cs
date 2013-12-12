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

    /// <summary>
    /// The statistic about requests done by a single client.
    /// </summary>
    internal class ClientStatistic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStatistic"/> class.
        /// </summary>
        public ClientStatistic()
        {
            this.LastRequest = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets or sets the date and time of the last request.
        /// </summary>
        public DateTime LastRequest { get; set; }

        /// <summary>
        /// Gets the requests per second.
        /// </summary>
        public int RequestsPerSecond { get; private set; }

        /// <summary>
        /// Increases the request count.
        /// </summary>
        public void IncreaseRequestCount()
        {
            if (this.RequestsPerSecond > 1 && this.LastRequest < DateTime.UtcNow.AddSeconds(-10))
            {
                this.RequestsPerSecond = this.RequestsPerSecond / 2;
            }

            this.LastRequest = DateTime.UtcNow;
            this.RequestsPerSecond++;
        }
    }
}