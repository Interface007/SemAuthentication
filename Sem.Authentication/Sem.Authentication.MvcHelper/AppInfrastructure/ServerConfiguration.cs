// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServerConfiguration.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ServerConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System.Xml.Serialization;

    /// <summary>
    /// The server parameter configuration.
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// Gets or sets the client id (for authenticating at the validation server).
        /// </summary>
        [XmlAttribute]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the API key (for authenticating at the validation server).
        /// </summary>
        [XmlAttribute]
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the sync level of the authentication server.
        /// </summary>
        [XmlAttribute]
        public string SyncLevel { get; set; }
    }
}