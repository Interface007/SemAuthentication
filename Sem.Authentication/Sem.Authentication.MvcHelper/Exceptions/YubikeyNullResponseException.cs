﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyNullResponseException.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyNullResponseException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Exceptions
{
    using System;

    /// <summary>
    /// The YUBIKEY null response exception will be thrown when the response of the validation is NULL.
    /// </summary>
    public class YubikeyNullResponseException : Exception
    {
    }
}