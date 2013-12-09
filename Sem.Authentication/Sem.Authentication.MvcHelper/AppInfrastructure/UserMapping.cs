// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserMapping.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The user mapping.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System.Xml.Serialization;

    /// <summary>
    /// The user mapping.
    /// </summary>
    public struct UserMapping
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the external id of the user (the one from the OTP system).
        /// </summary>
        [XmlAttribute]
        public string ExternalId { get; set; }
    }
}