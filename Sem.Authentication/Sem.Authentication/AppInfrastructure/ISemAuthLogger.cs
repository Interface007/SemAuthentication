// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISemAuthLogger.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ISemAuthLogger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using System;

    /// <summary>
    /// The logging interface you should implement to mal logging calls to your preferred logging library.
    /// </summary>
    public interface ISemAuthLogger
    {
        /// <summary>
        /// Los the the exception <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception"> The exception to be logged. </param>
        void Log(Exception exception);
    }
}
