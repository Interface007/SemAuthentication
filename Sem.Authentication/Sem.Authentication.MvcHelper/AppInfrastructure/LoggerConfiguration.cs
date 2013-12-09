// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerConfiguration.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the LoggerConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System.Xml.Serialization;

    /// <summary>
    /// The configuration for the logging functionality.
    /// </summary>
    public class LoggerConfiguration
    {
        /// <summary>
        /// Gets or sets the type name of the logger to use.
        /// </summary>
        [XmlAttribute]
        public string TypeName { get; set; }
    }
}