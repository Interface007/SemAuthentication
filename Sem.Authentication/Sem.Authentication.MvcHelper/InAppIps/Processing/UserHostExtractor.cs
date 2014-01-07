// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserHostExtractor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the UserHostExtractor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps.Processing
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
            if (context == null)
            {
                return string.Empty;
            }

            var request = context.Request;
            return request != null && request.Headers != null ? string.Join("-", request.UserHostAddress, request.Headers["REMOTE_ADDR"], GetFirstPart(request.Headers["HTTP_X_FORWARDED_FOR"])) :
                   request != null ? request.UserHostAddress 
                                   : string.Empty;
        }

        private static string GetFirstPart(string value)
        {
            value = value ?? string.Empty;
            return value.Contains(",") ? value.Substring(0, value.IndexOf(',')) : value;
        }
    }
}