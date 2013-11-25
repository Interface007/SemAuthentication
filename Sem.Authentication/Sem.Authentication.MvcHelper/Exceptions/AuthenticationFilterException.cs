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

    /// <summary>
    /// The authentication filter exception.
    /// </summary>
    public class AuthenticationFilterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFilterException"/> class.
        /// </summary>
        public AuthenticationFilterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationFilterException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        protected AuthenticationFilterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}