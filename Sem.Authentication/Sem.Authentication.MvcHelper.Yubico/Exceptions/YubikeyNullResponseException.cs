// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyNullResponseException.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyNullResponseException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    using Sem.Authentication.MvcHelper.Exceptions;

    /// <summary>
    /// The YUBIKEY null response exception will be thrown when the response of the validation is NULL.
    /// </summary>
    [Serializable]
    public class YubikeyNullResponseException : AuthenticationFilterException
    {
        public YubikeyNullResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNullResponseException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        [ExcludeFromCodeCoverage]
        public YubikeyNullResponseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNullResponseException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="innerException"> The innerException. </param>
        [ExcludeFromCodeCoverage]
        public YubikeyNullResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNullResponseException"/> class.
        /// </summary>
        /// <param name="info"> The info. </param>
        /// <param name="context"> The context. </param>
        [ExcludeFromCodeCoverage]
        protected YubikeyNullResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}