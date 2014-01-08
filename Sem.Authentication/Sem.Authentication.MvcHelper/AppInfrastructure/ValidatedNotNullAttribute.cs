// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatedNotNullAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ValidatedNotNullAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}