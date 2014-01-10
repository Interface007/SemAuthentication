// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IYubicoClient.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the IYubicoClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Client
{
    using YubicoDotNetClient;

    /// <summary>
    /// This interface enables us to wrap the YUBICO client, so that we can 
    /// use a MOQ to build a test double instead of communicating with the
    /// real server.
    /// </summary>
    public interface IYubicoClient
    {
        /// <summary>
        /// Gets or sets the client id from the information you got from YUBICO.
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the API key from the information you got from YUBICO.
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the sync level.
        /// </summary>
        string SyncLevel { get; set; }

        /// <summary>
        /// This method will validate an OTP.
        /// </summary>
        /// <param name="onetimePassword">The onetime password.</param>
        /// <returns>The <see cref="IYubicoResponse"/> indicating the result of the validation.</returns>
        IYubicoResponse Verify(string onetimePassword);
    }
}
