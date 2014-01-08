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

        /// <summary>
        /// Validates an argument to not be NULL.
        /// This method can be used as a guard validation that is also accepted as a null check by "Code Analysis".
        /// </summary>
        /// <param name="value"> The reference to the argument. </param>
        /// <param name="argumentName"> The name of the argument. </param>
        /// <param name="path"> The path to the source file (will be resolved automatically by the compiler). </param>
        /// <param name="line"> The line of the source file (will be resolved automatically by the compiler). </param>
        /// <typeparam name="T"> The type of the argument. </typeparam>
        /// <exception cref="ArgumentNullException"> In case of <paramref name="value"/> == null. </exception>
        public static void ArgumentMustNotBeNull<T>([ValidatedNotNull]this T value, string argumentName, [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    string.Format(CultureInfo.InvariantCulture, "The argument {2} of type {3} must not be null in file {0}, line {1}.", path, line, argumentName, typeof(T).Name),
                    argumentName);
            }
        }
    }
}
