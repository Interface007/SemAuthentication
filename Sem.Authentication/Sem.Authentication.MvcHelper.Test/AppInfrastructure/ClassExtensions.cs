// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassExtensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.AppInfrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.AppInfrastructure;
    using Sem.Authentication.MvcHelper.AppInfrastructure;

    /// <summary>
    /// Tests the methods of the class <see cref="Extensions"/>.
    /// </summary>
    public static class ClassExtensions
    {
        /// <summary>
        /// Tests the method <see cref="Extensions.EnsureCorrectConfiguration"/>.
        /// </summary>
        [TestClass]
        public class EnsureCorrectConfiguration
        {
            /// <summary>
            /// The method should throw an exception if the configuration is null.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfConfigurationIsNull()
            {
                (null as ConfigurationBase).EnsureCorrectConfiguration();
            }

            /// <summary>
            /// The method should throw an exception if an exception has been set inside the configuration base instance.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfExceptionIsSet()
            {
                var config = new ConfigurationBase
                    {
                        Exception = new ArgumentOutOfRangeException()
                    };

                config.EnsureCorrectConfiguration();
            }

            /// <summary>
            /// The method should not throw an exception if the configuration is ok.
            /// </summary>
            [TestMethod]
            public void ThrowsNotIfConfigurationIsOk()
            {
                var config = new ConfigurationBase();
                config.EnsureCorrectConfiguration();
            }
        }

        /// <summary>
        /// Tests the method <see cref="Extensions.ArgumentMustNotBeNull{T}"/>.
        /// </summary>
        [TestClass]
        public class ArgumentMustNotBeNull
        {
            /// <summary>
            /// The method should throw an exception if the argument is null.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfArgumentIsNull()
            {
                ((ArgumentMustNotBeNull)null).ArgumentMustNotBeNull("sample");
            }

            /// <summary>
            /// The method should NOT throw an exception if the argument is not null.
            /// </summary>
            [TestMethod]
            public void ThrowsNotIfArgumentIsNotNull()
            {
                this.ArgumentMustNotBeNull("sample");
            }
        }

        /// <summary>
        /// Tests the method <see cref="Extensions.ArgumentPropertyMustNotBeNull{T,TParameter}"/>.
        /// </summary>
        [TestClass]
        public class ArgumentPropertyMustNotBeNull
        {
            /// <summary>
            /// The method should throw an exception if the argument-property is null.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfArgumentIsNull()
            {
                new AuditInfo<Exception>("name", "action").ArgumentPropertyMustNotBeNull("sample", "Details", x => x.Details);
            }

            /// <summary>
            /// The method should NOT throw an exception if the argument-property is not null.
            /// </summary>
            [TestMethod]
            public void ThrowsNotIfArgumentIsNotNull()
            {
                new AuditInfo<Exception>("name", "action").ArgumentPropertyMustNotBeNull("sample", "User", x => x.User);
            }
        }
    }
}
