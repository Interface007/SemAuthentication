// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassAuthenticationCheck.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassAuthenticationCheck type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.AppInfrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    public static class ClassAuthenticationCheck
    {
        [TestClass]
        public class Auditing
        {
            [TestMethod]
            public void CreatesNoAuditByDefault()
            {
                var configuration = new ConfigurationBase();
                var target = new SampleAuthenticator(configuration);
                target.AuditEvent(new Exception("Sample"), MvcTestBase.CreateRequestContext());
                var result = target.InternalAudit;

                Assert.IsNull(result);
            }

            [TestMethod]
            public void CreatesCallsAuditForFailureByDefault()
            {
                var configuration = new ConfigurationBase
                    {
                        Audit = new TypeConfiguration
                        {
                            TypeName = "Sem.Authentication.MvcHelper.AppInfrastructure.DebugAudit, Sem.Authentication.MvcHelper",
                        }
                    };

                var target = new SampleAuthenticator(configuration);
                target.AuditEvent(new Exception("Sample"), MvcTestBase.CreateRequestContext());
                var result = target.InternalAudit;

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void CreatesCallsAuditForSuccessByDefault()
            {
                var configuration = new ConfigurationBase
                    {
                        Audit = new TypeConfiguration
                        {
                            TypeName = "Sem.Authentication.MvcHelper.AppInfrastructure.DebugAudit, Sem.Authentication.MvcHelper",
                        }
                    };

                var target = new SampleAuthenticator(configuration);
                target.AuditEvent(MvcTestBase.CreateRequestContext());
                var result = target.InternalAudit;

                Assert.IsNotNull(result);
            }
        }

        [TestClass]
        public class Logging
        {
            [TestMethod]
            public void CreatesStandardLogger()
            {
                var configuration = new ConfigurationBase
                    {
                        Logger = new TypeConfiguration
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
                        Logger = new TypeConfiguration
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
            [ExcludeFromCodeCoverage]
            public void ThrowsIfContextIsNull()
            {
                var target = new SampleAuthenticator(null);
                target.OnActionExecuting(null);
            }

            [TestMethod]
            public void RedirectsIfExceptionAndActionIsSet()
            {
                // todo: still having issues testing this: the newly generated UrlHelper needs "something" to build up a URL
                var context = MvcTestBase.CreateRequestContext(new Uri("http://exception/Home/Index"));
                var target = new SampleAuthenticator(new ConfigurationBase()) { InvalidKeyAction = "Index" };
                target.OnActionExecuting(context);
                var redirectResult = context.Result as RedirectResult;
                Assert.IsNotNull(redirectResult);
                Assert.AreEqual("/Home/Index/", redirectResult.Url);
            }

            /// <summary>
            /// Because not initializing the http context will result in this property returning an <c>EmptyHttpContext</c> 
            /// (an internal type, so we cannot easily test for it), we will not get the expected <see cref="ArgumentNullException"/>
            /// but an <see cref="NotImplementedException"/> thrown by this "dummy context".
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(NotImplementedException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsIfContextHttpContextIsNull()
            {
                var target = new SampleAuthenticator(null);
                target.OnActionExecuting(new ActionExecutingContext());
            }

            [TestMethod]
            public void ContinuesProcessingWhenUrlIsNull()
            {
                var context = MvcTestBase.CreateRequestContext((Uri)null);

                var target = new SampleAuthenticator(new ConfigurationBase()) { ImageOnly = true, };
                target.OnActionExecuting(context);
            }
        }
    }
}