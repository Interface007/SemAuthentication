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

    using Sem.Authentication.InAppIps;

    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time. 
    /// This attribute does enforce a minimum time between two requests from the same IP.
    /// </summary>
    public sealed class MinimumRequestTimeDistanceAttribute : BaseGateMvcAttribute
    {
        private readonly MinimumRequestTimeDistance gate = new MinimumRequestTimeDistance();

        public MinimumRequestTimeDistanceAttribute()
        {
        }

        public MinimumRequestTimeDistanceAttribute(params Type[] extractors)
            : base(extractors)
        {
        }

        public string Name
        {
            get
            {
                return this.gate.Name;
            }
           
            set
            {
                this.gate.Name = value;
            }
        }

        public string Message
        {
            get
            {
                return this.gate.Message;
            }
           
            set
            {
                this.gate.Message = value;
            }
        }

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

        public override IGate Instance
        {
            get
            {
                return this.gate;
            }
        }
    }
}
