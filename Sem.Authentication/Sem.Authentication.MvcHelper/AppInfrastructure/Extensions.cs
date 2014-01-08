// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the Extensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extension methods for the configuration object.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks the <see cref="Exception"/> property for deserialization issues and throws an exception if it's not NULL.
        /// </summary>
        /// <param name="configuration"> The configuration to check. </param>
        public static void EnsureCorrectConfiguration(this ConfigurationBase configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration", "The configuration has not been set up properly. Look at https://semauthentication.codeplex.com/ for more information on how to use this library.");
            }

            if (configuration.Exception != null)
            {
                throw configuration.Exception;
            }
        }

        public static void ArgumentMustNotBeNull<T>([ValidatedNotNull]this T value, string argumentName, [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    string.Format(CultureInfo.InvariantCulture, "The argument must not be null in file {0}, line {1}.", path, line),
                    argumentName);
            }
        }
    }
}
