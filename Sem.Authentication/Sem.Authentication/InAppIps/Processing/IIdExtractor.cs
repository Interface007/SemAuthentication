// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdExtractor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the IIdExtractor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps.Processing
{
    using System.Web;

    /// <summary>
    /// The interface to get the client ID.
    /// </summary>
    public interface IIdExtractor
    {
        /// <summary>
        /// Needs to implement a functionality to get a quasi-unique ID of the client.
        /// </summary>
        /// <param name="context"> The <see cref="HttpContext"/> to extract the ID from. </param>
        /// <returns> The client ID. This value will be used as a key to store the client statistics. </returns>
        string Extract(HttpContextBase context);
    }
}