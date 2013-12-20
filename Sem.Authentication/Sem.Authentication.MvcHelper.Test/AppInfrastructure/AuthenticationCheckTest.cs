namespace Sem.Authentication.MvcHelper.Test.AppInfrastructure
{
    using System;
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    public static class AuthenticationCheckTest
    {
        [TestClass]
        public class Loggong
        {
            [TestMethod]
            public void CreatesStandardLogger()
            {
                var configuration = new ConfigurationBase
                                        {
                                            Logger = new LoggerConfiguration
                                                         {
                                                             TypeName = "Sem.Authentication.MvcHelper.AppInfrastructure.DebugLogger, Sem.Authentication.MvcHelper",
                                                         }
                                        };

                var target = new SampleAuthenticator(configuration);
                target.LogException(new Exception("Sample"));
                var result = target.InternalLogger;
                
                Assert.IsNotNull(result);
            }
        
            [TestMethod]
            public void DoesNotThrowExceptionsIfLoggerNotCreatable()
            {
                var configuration = new ConfigurationBase
                                        {
                                            Logger = new LoggerConfiguration
                                                         {
                                                             TypeName = "Sem.Authentication.MvcHelper.A598D4BDB7244B62A4A158201C118882, Sem.Authentication.MvcHelper",
                                                         }
                                        };

                var target = new SampleAuthenticator(configuration);
                target.LogException(new Exception("Sample"));
                var result = target.InternalLogger;
                
                Assert.IsNull(result);
            }
        }

        [TestClass]
        public class OnActionExecuting
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfContextIsNull()
            {
                var target = new SampleAuthenticator(null);
                target.OnActionExecuting(null);
            }
        
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsIfContextHttpContextIsNull()
            {
                var target = new SampleAuthenticator(null);
                target.OnActionExecuting(new ActionExecutingContext());
            }
        }
    }
}