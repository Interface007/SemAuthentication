// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationBase.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ConfigurationBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// The configuration base class for authenticators.
    /// </summary>
    public class ConfigurationBase
    {
        /// <summary>
        /// Gets or sets the exception to throw as soon as there is someone that can catch it.
        /// </summary>
        [XmlIgnore]
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the logger to write logging information.
        /// </summary>
        public TypeConfiguration Logger { get; set; }

        /// <summary>
        /// Gets or sets the logger to write audit information.
        /// </summary>
        public TypeConfiguration Audit { get; set; }
    }
}