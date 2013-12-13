// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FastRequestsProtectionAttributeTest.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the Tests for the FastRequestsProtectionAttribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Sem.Authentication.MvcHelper.InAppIps;

    /// <summary>
    /// Contains tests for all methods of <see cref="FastRequestsProtectionAttribute"/>.
    /// </summary>
    [TestClass]
    public class FastRequestsProtectionAttributeTest
    {
        /// <summary>
        /// Tests the constructor of <see cref="FastRequestsProtectionAttribute"/>.
        /// </summary>
        [TestClass]
        public class Ctor
        {
            /// <summary>
            /// Tests whether the default constructor initializes the object with 30 seconds max. retention time.
            /// </summary>
            [TestMethod]
            public void InitializesWith30SecondsMaxRetentionTime()
            {
                var target = new FastRequestsProtectionAttribute();
                Assert.AreEqual(3000, target.MaxRetentionTimeOfStatistics);
            }
        }

        /// <summary>
        /// Tests for implementing the assumed interfaces.
        /// </summary>
        [TestClass]
        public class Interfaces
        {
            /// <summary>
            /// The class should inherit from <see cref="ActionFilterAttribute"/>.
            /// </summary>
            [TestMethod]
            public void ImplementsActionFilterAttribute()
            {
                var target = new FastRequestsProtectionAttribute();
                Assert.IsInstanceOfType(target, typeof(ActionFilterAttribute));
            }
        }

        /// <summary>
        /// Tests the method <see cref="FastRequestsProtectionAttribute.OnActionExecuting"/>.
        /// </summary>
        [TestClass]
        public class OnActionExecuting
        {
            /// <summary>
            /// Tests whether the attribute blocks the configured fast calls by throwing exceptions.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(HttpException))]
            public void BlocksConfiguredFastCalls()
            {
                // we setup a request context that returns always the same session id
                var rcontext = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };
                
                // calling 3 times should throw an exception
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
            }
         
            /// <summary>
            /// Tests whether the attribute blocks the configured fast calls by throwing exceptions
            /// even if the requests are being handled by different instances of the attribute.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(HttpException))]
            public void BlocksConfiguredFastCallsOnTwoInstances()
            {
                // we setup a request context that returns always the same session id
                var rcontext = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target1 = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };
                var target2 = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };
                
                // calling 3 times should throw an exception
                target1.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target2.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target1.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
            }

            /// <summary>
            /// Tests whether the attribute blocks the configured fast calls by throwing exceptions.
            /// </summary>
            [TestMethod]
            public void NotBlocksLessThanConfiguredFastCalls()
            {
                var rcontext = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };
                
                // calling 2 times should NOT throw an exception
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });

                Assert.IsNotNull(target);
            }

            /// <summary>
            /// Tests whether the attribute NOT blocks the configured fast calls when pausing according to the configuration.
            /// </summary>
            /// <returns> The <see cref="Task"/> to test. </returns>
            [TestMethod]
            public async Task NotBlocksMultipleLessThanConfiguredFastCalls()
            {
                // we setup a request context that returns always the same session id
                var rcontext = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute
                                 {
                                     RequestsPerSecondAndClient = 2,
                                     MaxRetentionTimeOfStatistics = 50,
                                 };
                
                // calling 2 times should NOT throw an exception
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });

                await Task.Delay(100);

                // calling 2 times should NOT throw an exception
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });

                Assert.IsNotNull(target);
            }

            /// <summary>
            /// Creates a new request context that returns a distinct, but always the same, session id.
            /// </summary>
            /// <returns> The <see cref="Mock"/> object for the request context. </returns>
            private static Mock<RequestContext> CreateRequestContext()
            {
                // we setup a request context that returns always the same session id
                var sessionState = new Mock<HttpSessionStateBase>();
                sessionState.Setup(x => x.SessionID).Returns(Guid.NewGuid().ToString("N"));
                
                var httpContext = new Moq.Mock<HttpContextBase>();
                httpContext.Setup(x => x.Session).Returns(sessionState.Object);
                
                var requestContext = new Moq.Mock<RequestContext>();
                requestContext.Setup(x => x.HttpContext).Returns(httpContext.Object);
                
                return requestContext;
            }
        }
    }
}
