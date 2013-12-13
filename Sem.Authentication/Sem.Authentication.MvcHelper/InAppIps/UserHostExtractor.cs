// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserHostExtractor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the UserHostExtractor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System.Web;

    /// <summary>
    /// The user host extractor returns the clients IP address as the client ID.
    /// </summary>
    public class UserHostExtractor : IIdExtractor
    {
        /// <summary>
        /// Extracts the clients IP address.
        /// </summary>
        /// <param name="context"> The http context to get the client ID from. </param>
        /// <returns> The <see cref="string"/> representing the "unique" id of the client. </returns>
        public string Extract(HttpContextBase context)
        {
            return context.Request == null ? string.Empty : context.Request.UserHostAddress;
        }
    }
}