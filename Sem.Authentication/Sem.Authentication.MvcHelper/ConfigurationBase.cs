// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationBase.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ConfigurationBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System;
    using System.Xml.Serialization;

    public class ConfigurationBase
    {
        /// <summary>
        /// Gets or sets the exception to throw as soon as there is someone that can catch it.
        /// </summary>
        [XmlIgnore]
        public Exception Exception { get; set; }

        public LoggerConfiguration Logger { get; set; }

        /// <summary>
        /// Checks the <see cref="Exception"/> property for deserialization issues and throws an exception if it's not NULL.
        /// </summary>
        public void EnsureCorrectConfiguration()
        {
            if (this.Exception != null)
            {
                throw this.Exception;
            }
        }
    }
}