// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeConfiguration.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the TypeConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System.Xml.Serialization;

    /// <summary>
    /// The configuration for the logging functionality.
    /// </summary>
    public class TypeConfiguration
    {
        /// <summary>
        /// Gets or sets the type name of the logger to use.
        /// </summary>
        [XmlAttribute]
        public string TypeName { get; set; }
    }
}