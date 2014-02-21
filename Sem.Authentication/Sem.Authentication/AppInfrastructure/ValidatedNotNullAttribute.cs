// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatedNotNullAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ValidatedNotNullAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using System;

    /// <summary>
    /// This attribute will be detected by the code analysis and will tag variables passed to methods 
    /// as not being null. This will prevent CA1062.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}