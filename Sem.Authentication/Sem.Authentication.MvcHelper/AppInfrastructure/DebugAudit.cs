// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugAudit.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the DebugAudit type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.AppInfrastructure
{
    using System.Diagnostics;

    /// <summary>
    /// A sample implementation for <see cref="ISemAudit"/> that simply logs to the debug output.
    /// </summary>
    public class DebugAudit : ISemAudit
    {
        /// <summary>
        /// Writes information about the failure of an action into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details.  </param>
        public void AuthenticationCheckFailed<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            Debug.Print("FAILURE: User {0} did {1} - exception: {2}", info.User, info.Action, info.Details);
        }

        /// <summary>
        /// Writes information about successes into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details. </param>
        public void AuthenticationCheckSucceeded<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            Debug.Print("SUCCESS: User {0} did {1}", info.User, info.Action);
        }
    }
}