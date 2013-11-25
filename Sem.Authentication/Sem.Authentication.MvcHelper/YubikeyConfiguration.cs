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
        /// Gets or sets the user mappings.
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