// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyConfiguration.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web;
    using System.Xml.Serialization;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    /// <summary>
    /// The YUBIKEY configuration.
    /// </summary>
    public class YubikeyConfiguration : ConfigurationBase
    {
        /// <summary>
        /// Gets or sets the server configuration for validation.
        /// </summary>
        public ServerConfiguration Server { get; set; }

        /// <summary>
        /// Gets or sets the user mappings. Here you need to map the ASP.Net user name to an external YUBIKEY-ID (the first 12 
        /// characters from a standard YUBIKEY token). Access will be granted only if this external ID has been mapped to the 
        /// current user. 
        /// </summary>
        [XmlArrayItem("User")]
        public List<UserMapping> Users { get; set; }

        /// <summary>
        /// Deserializes the configuration from a file <c>YubiKey.xml</c> in the application path of the app domain.
        /// </summary>
        /// <returns> The <see cref="YubikeyConfiguration"/>. </returns>
        [ExcludeFromCodeCoverage]
        public static YubikeyConfiguration DeserializeConfiguration()
        {
            var serializer = new XmlSerializer(typeof(YubikeyConfiguration));
            var path = Path.Combine(HttpRuntime.AppDomainAppPath, "YubiKey.xml");

            if (!File.Exists(path))
            {
                return new YubikeyConfiguration { Exception = new FileNotFoundException(string.Format("The configuration file YubiKey.xml cannot be found. You need to place this file into the path {0} to make this application work. Have a look at https://semauthentication.codeplex.com/ for details about this module.", HttpRuntime.AppDomainAppPath)) };
            }

            using (var file = File.OpenRead(path))
            {
                return (YubikeyConfiguration)serializer.Deserialize(file);
            }
        }
    }
}