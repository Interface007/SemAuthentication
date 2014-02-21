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

    using Sem.Authentication.InAppIps;
    using Sem.Authentication.InAppIps.Processing;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute protects against multiple fast requests from a single client.
    /// </summary>
    public sealed class FastRequestsProtectionAttribute : BaseGateMvcAttribute
    {
        /// <summary>
        /// The gate instance.
        /// </summary>
        private readonly FastRequestsProtection instance = new FastRequestsProtection();

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
            : base(extractors)
        {
        }

        /// <summary>
        /// Gets the instance that implements the functionality of this gate.
        /// </summary>
        public override IGate Instance
        {
            get
            {
                return this.instance;
            }
        }

        /// <summary>
        /// Gets or sets the maximum retention time in milliseconds of statistics - when a client does not issue requests in this period of time, we will remove the statistic.
        /// </summary>
        public int MaxRetentionTimeOfStatistics
        {
            get
            {
                return this.instance.MaxRetentionTimeOfStatistics;
            }

            set
            {
                this.instance.MaxRetentionTimeOfStatistics = value;
            }
        }

        /// <summary>
        /// Gets or sets the requests per second the client is allowed to do.
        /// </summary>
        public int RequestsPerSecondAndClient
        {
            get
            {
                return this.instance.RequestsPerSecondAndClient;
            }

            set
            {
                this.instance.RequestsPerSecondAndClient = value;
            }
        }
    }
}