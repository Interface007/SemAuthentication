// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventLogAudit.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   A sample implementation for <see cref="ISemAudit" /> that simply logs to the windows event log.
//   Make sure that the application user can write to the event log and can create the event source.
//   You need to grant read access to all event logs.
//   NOT UNIT-TESTED! This code is not testable with unit testing, because this is the interaction
//   with the event log (code that heavily relies on static methods of the class "EventLog").
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.AppInfrastructure
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// A sample implementation for <see cref="ISemAudit"/> that simply logs to the windows event log.
    /// Make sure that the application user can write to the event log and can create the event source.
    /// You need to grant read access to all event logs.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class EventLogAudit : ISemAudit
    {
        /// <summary>
        /// The name of the event source.
        /// </summary>
        private static readonly string EventSourceName = typeof(EventLogAudit).Namespace;

        /// <summary>
        /// Initializes static members of the <see cref="EventLogAudit"/> class.
        /// </summary>
        static EventLogAudit()
        {
            try
            {
                if (!EventLog.SourceExists(EventSourceName))
                {
                    EventLog.CreateEventSource(EventSourceName, EventSourceName);
                }
            }
            catch (System.Security.SecurityException ex)
            {
                const string Message = "There is an issue with checking or creating the EventSource for writing audit information into the event log. "
                                     + "You should check permissions for the technical user this application runs with. "
                                     + "See https://semauthentication.codeplex.com/wikipage?title=EventLogAudit%20Security%20Exception&referringTitle=Documentation for more information about this error.";

                throw new InvalidOperationException(Message, ex);
            }
        }

        /// <summary>
        /// Writes information about the failure of an action into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details.  </param>
        public void AuthenticationCheckFailed<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            var message = string.Format(CultureInfo.InvariantCulture, "FAILURE: User {0} did {1} - exception: {2}", info.User, info.Action, info.Details);
            EventLog.WriteEntry(EventSourceName, message, EventLogEntryType.Warning, 101);
        }

        /// <summary>
        /// Writes information about successes into the audit log.
        /// </summary>
        /// <typeparam name="T"> The type of detail data </typeparam>
        /// <param name="info"> The information about the user, the action and the details. </param>
        public void AuthenticationCheckSucceeded<T>(AuditInfo<T> info)
        {
            info.ArgumentMustNotBeNull("info");
            var message = string.Format(CultureInfo.InvariantCulture, "SUCCESS: User {0} did {1}", info.User, info.Action);
            EventLog.WriteEntry(EventSourceName, message, EventLogEntryType.Information, 102);
        }
    }
}