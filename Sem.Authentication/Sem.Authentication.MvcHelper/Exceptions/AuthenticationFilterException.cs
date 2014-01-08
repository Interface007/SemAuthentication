// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationFilterException.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The authentication filter exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The authentication filter exception.
    /// </summary>
    [Serializable]
    public abstract class AuthenticationFilterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFilterException"/> class.
        /// </summary>
        protected AuthenticationFilterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFilterException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        [ExcludeFromCodeCoverage]
        protected AuthenticationFilterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFilterException"/> class.
        /// </summary>
        /// <param name="message"> The message. </param>
        /// <param name="innerException"> The inner exception. </param>
        protected AuthenticationFilterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFilterException"/> class.
        /// </summary>
        /// <param name="info"> The info. </param>
        /// <param name="context"> The context. </param>
        [ExcludeFromCodeCoverage]
        protected AuthenticationFilterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}