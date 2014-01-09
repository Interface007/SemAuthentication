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

    public class DebugAudit : ISemAudit
    {
        public void AuthenticationCheckFailed<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            Debug.Print("FAILURE: User {0} did {1} - exception: {2}", info.User, info.Action, info.Details);
        }

        public void AuthenticationCheckSucceeded<T>(AuditInfo<T> info)
        {
            Debug.Print("SUCCESS: User {0} did {1}", info.User, info.Action);
        }
    }
}