// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LandmineMvcAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the LandmineAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System;

    using Sem.Authentication.InAppIps;

    public class LandmineMvcAttribute : BaseGateMvcAttribute
    {
        private readonly Landmine landmineAttribute = new Landmine();

        public LandmineMvcAttribute()
        {
        }

        public LandmineMvcAttribute(params Type[] extractors)
            : base(extractors)
        {
        }

        public string LandmineName
        {
            get
            {
                return this.landmineAttribute.LandmineName;
            }
           
            set
            {
                this.landmineAttribute.LandmineName = value;
            }
        }

        public string ExpectedValue
        {
            get
            {
                return this.landmineAttribute.ExpectedValue;
            }
            
            set
            {
                this.landmineAttribute.ExpectedValue = value;
            }
        }

        /// <summary>
        /// Gets the instance that implements the functionality of this gate.
        /// </summary>
        public override IGate Instance
        {
            get
            {
                return this.landmineAttribute;
            }
        }
    }
}