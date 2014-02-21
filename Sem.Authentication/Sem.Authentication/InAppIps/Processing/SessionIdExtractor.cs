// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionIdExtractor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the SessionIdExtractor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps.Processing
{
    using System.Web;

    /// <summary>
    /// The session id extractor extracts the session ID from the <see cref="HttpContext"/>.
    /// </summary>
    public class SessionIdExtractor : IIdExtractor
    {
        /// <summary>
        /// Extracts the clients session id.
        /// </summary>
        /// <param name="context"> The http context to get the session id from. </param>
        /// <returns> The <see cref="string"/> representing the "unique" id of the client. </returns>
        public string Extract(HttpContextBase context)
        {
            return context == null || context.Session == null ? string.Empty : context.Session.SessionID;
        }
    }
}