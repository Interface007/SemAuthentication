// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LandmineWebApiAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the LandmineWebApiAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.WebApiHelper.InAppIps
{
    using System;

    using Sem.Authentication.InAppIps;

    /// <summary>
    /// The WebAPI implementation of the landmine attribute.
    /// </summary>
    public class LandmineWebApiAttribute : BaseGateWebApiAttribute
    {
        /// <summary>
        /// The landmine gate.
        /// </summary>
        private readonly Landmine gate = new Landmine();

        /// <summary>
        /// Initializes a new instance of the <see cref="LandmineWebApiAttribute"/> class.
        /// </summary>
        public LandmineWebApiAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandmineWebApiAttribute"/> class.
        /// </summary>
        /// <param name="extractors"> The extractors to use for identifying the client. </param>
        public LandmineWebApiAttribute(params Type[] extractors)
            : base(extractors)
        {
        }

        /// <summary>
        /// Gets or sets the name of the land mine this attribute belongs to.
        /// </summary>
        public string LandmineName
        {
            get
            {
                return this.gate.LandmineName;
            }
           
            set
            {
                this.gate.LandmineName = value;
            }
        }

        /// <summary>
        /// Gets or sets the expected value for the land mine.
        /// </summary>
        public string ExpectedValue
        {
            get
            {
                return this.gate.ExpectedValue;
            }
            
            set
            {
                this.gate.ExpectedValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of seconds clients must wait before the user will be 
        /// able to call this method again after triggering the land mine.
        /// </summary>
        public int Seconds
        {
            get
            {
                return this.gate.Seconds;
            }
            
            set
            {
                this.gate.Seconds = value;
            }
        }

        /// <summary>
        /// Gets or sets the request area where to search for the landmine value.
        /// </summary>
        public RequestArea RequestArea
        {
            get
            {
                return this.gate.RequestArea;
            }

            set
            {
                this.gate.RequestArea = value;
            }
        }

        /// <summary>
        /// Gets the instance that implements the functionality of this gate.
        /// </summary>
        public override IGate Instance
        {
            get
            {
                return this.gate;
            }
        }
    }
}