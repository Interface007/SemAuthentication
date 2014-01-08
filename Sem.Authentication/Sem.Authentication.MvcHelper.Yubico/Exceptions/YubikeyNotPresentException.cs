// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyNotPresentException.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyNotPresentException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    using Sem.Authentication.MvcHelper.Exceptions;

    /// <summary>
    /// The YUBIKEY not present exception will be thrown when the key value is not present inside the request.
    /// </summary>
    [Serializable]
    public class YubikeyNotPresentException : AuthenticationFilterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNotPresentException"/> class.
        /// </summary>
        public YubikeyNotPresentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNotPresentException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        [ExcludeFromCodeCoverage]
        public YubikeyNotPresentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNotPresentException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="innerException"> The inner exception. </param>
        [ExcludeFromCodeCoverage]
        public YubikeyNotPresentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyNotPresentException"/> class.
        /// </summary>
        /// <param name="info"> The info. </param>
        /// <param name="context"> The context. </param>
        [ExcludeFromCodeCoverage]
        protected YubikeyNotPresentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}