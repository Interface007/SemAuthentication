// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyInvalidResponseException.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyInvalidResponseException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Exceptions
{
    using System;

    using YubicoDotNetClient;

    /// <summary>
    /// The YUBIKEY invalid response exception.
    /// </summary>
    public class YubikeyInvalidResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyInvalidResponseException"/> class.
        /// </summary>
        /// <param name="status"> The response status. </param>
        public YubikeyInvalidResponseException(YubicoResponseStatus status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets the response status.
        /// </summary>
        public YubicoResponseStatus Status { get; set; }
    }
}