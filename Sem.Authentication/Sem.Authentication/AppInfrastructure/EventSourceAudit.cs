// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSourceAudit.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   A sample implementation for  that simply logs to the windows event log.
//   Make sure that the application user can write to the event log and can create the event source.
//   You need to grant read access to all event logs.
//   NOT UNIT-TESTED! This code is not testable with unit testing, because this is the interaction
//   with the event log (code that heavily relies on static methods of the class "EventLog").
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Tracing;
    using System.Reflection;

    /// <summary>
    /// A sample implementation for <see cref="ISemAudit"/> that simply logs to the windows event log.
    /// Make sure that the application user can write to the event log and can create the event source.
    /// You need to grant read access to all event logs.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EventSource(Name = "Sem.Authentication")]
    public class EventSourceAudit : EventSource, ISemAudit
    {
        /// <summary>
        /// Writes information about the failure of an action into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details.  </param>
        [Event(1, Message = "Application Failure: User {0} did {1} - exception: {2}", Level = EventLevel.Warning)]
        public void AuthenticationCheckFailed<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            this.WriteEvent(1, info.User, info.Action, info.Details);
        }

        /// <summary>
        /// Writes information about successes into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details. </param>
        [Event(2, Message = "User {0} did {1}", Level = EventLevel.Informational)]
        public void AuthenticationCheckSucceeded<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            this.WriteEvent(2, info.User, info.Action);
        }

        public void Register()
        {
            var source = this.GetType().GetCustomAttribute<EventSourceAttribute>().Name;
            if (EventLog.SourceExists(source))
            {
                EventLog.DeleteEventSource(source);
                EventLog.WriteEntry(source, string.Format("Event Log Created '{0}'", source), EventLogEntryType.Information);
            }

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, source);
                EventLog.WriteEntry(source, string.Format("Event Log Created '{0}'", source), EventLogEntryType.Information);
            }
        }
    }
}