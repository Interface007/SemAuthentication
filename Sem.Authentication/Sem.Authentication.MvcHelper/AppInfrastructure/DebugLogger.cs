// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugLogger.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the DebugLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The sample debug logger implementation. This logger implementation does simply print the information to the debug window of Visual Studio.
    /// </summary>
    public class DebugLogger : ISemAuthLogger
    {
        /// <summary>
        /// Los the the exception <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception"> The exception to be logged. </param>
        public void Log(Exception exception)
        {
            exception.ArgumentMustNotBeNull("exception");
            Debug.Print(exception.ToString());
        }
    }
}