// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerConfiguration.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the LoggerConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System.Xml.Serialization;

    public class LoggerConfiguration
    {
        [XmlAttribute]
        public string TypeName { get; set; }
    }
}