// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISemAudit.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ISemAudit type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using Sem.Authentication.MvcHelper.AppInfrastructure;

    /// <summary>
    /// In contrast to the logging, auditing does provide information about what did who when.
    /// The aim of auditing is non-repudiation.
    /// </summary>
    public interface ISemAudit
    {
        /// <summary>
        /// Writes information about the failure of an action into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details.  </param>
        void AuthenticationCheckFailed<T>(AuditInfo<T> info);

        /// <summary>
        /// Writes information about successes into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details. </param>
        void AuthenticationCheckSucceeded<T>(AuditInfo<T> info);
    }
}
