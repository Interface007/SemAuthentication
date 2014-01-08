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

    using Sem.Authentication.MvcHelper.Exceptions;

    /// <summary>
    /// The YUBIKEY not present exception will be thrown when the key value is not present inside the request.
    /// </summary>
    [Serializable]
    public class YubikeyNotPresentException : AuthenticationFilterException
    {
    }
}