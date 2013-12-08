// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientStatistic.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClientStatistic type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System;

    internal class ClientStatistic
    {
        private int requestCount;

        public ClientStatistic()
        {
            this.LastRequest = DateTime.UtcNow;
        }

        public void IncreaseRequestCount()
        {
            if (this.requestCount > 1 && this.LastRequest < DateTime.UtcNow.AddSeconds(10))
            {
                this.requestCount = this.requestCount / 2;
            }

            this.LastRequest = DateTime.UtcNow;
            this.requestCount++;
        }

        public DateTime LastRequest { get; set; }

        public int RequestsPerSecond
        {
            get
            {
                return this.requestCount;
            }
        }
    }
}