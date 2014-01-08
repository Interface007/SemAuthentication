namespace Sem.Authentication.MvcHelper.Test.AppInfrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    public static class ClassExtensions
    {
        [TestClass]
        public class EnsureCorrectConfiguration
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfConfigurationIsNull()
            {
                var config = null as ConfigurationBase;
                config.EnsureCorrectConfiguration();
            }

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

            [TestMethod]
            public void ThrowsNotIfConfigurationIsOk()
            {
                var config = new ConfigurationBase();
                config.EnsureCorrectConfiguration();
            }
        }

        [TestClass]
        public class ArgumentMustNotBeNull
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfArgumentIsNull()
            {
                ((ArgumentMustNotBeNull)null).ArgumentMustNotBeNull("sample");
            }
        
            [TestMethod]
            public void ThrowsNotIfArgumentIsNotNull()
            {
                this.ArgumentMustNotBeNull("sample");
            }
        }
    }
}
