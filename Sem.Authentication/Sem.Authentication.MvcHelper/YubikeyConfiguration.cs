// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyConfiguration.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml.Serialization;

    /// <summary>
    /// The YUBIKEY configuration.
    /// </summary>
    public class YubikeyConfiguration
    {
        /// <summary>
        /// Gets or sets the server configuration for validation.
        /// </summary>
        public ServerConfiguration Server { get; set; }

        /// <summary>
        /// Gets or sets the user mappings. Here you need to map the ASP.Net user name to an external Yubikey-ID (the first 12 
        /// characters from a standard Yubikey token). Access will be granted only if this external ID has been mapped to the 
        /// current user. 
        /// </summary>
        [XmlArrayItem("User")]
        public List<UserMapping> Users { get; set; }

        /// <summary>
        /// Deserializes the configuration from a file <c>YubiKey.xml</c> in the application path of the app domain.
        /// </summary>
        /// <returns> The <see cref="YubikeyConfiguration"/>. </returns>
        public static YubikeyConfiguration DeserializeConfiguration()
        {
            var serializer = new XmlSerializer(typeof(YubikeyConfiguration));
            var path = Path.Combine(HttpRuntime.AppDomainAppPath, "YubiKey.xml");
            using (var file = File.OpenRead(path))
            {
                return (YubikeyConfiguration)serializer.Deserialize(file);
            }
        }
    }
}