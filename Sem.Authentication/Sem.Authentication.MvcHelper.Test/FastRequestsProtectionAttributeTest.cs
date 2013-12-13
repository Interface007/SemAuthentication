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
    using System.Collections.Generic;
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
            public void NotBlocksConfiguredFastCallsWithoutClientId()
            {
                // we setup a request context that returns always the same session id
                var context = CreateRequestContext(string.Empty, string.Empty);

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };

                // calling 3 times should throw an exception
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);

                Assert.IsNotNull(target);
            }

            /// <summary>
            /// Tests whether the attribute blocks the configured fast calls by throwing exceptions.
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(HttpException))]
            public void BlocksConfiguredFastCalls()
            {
                // we setup a request context that returns always the same session id
                var context = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };

                // calling 3 times should throw an exception
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);
            }

            /// <summary>
            /// Tests whether the attribute blocks the configured fast calls by throwing exceptions.
            /// </summary>
            [TestMethod]
            public void RedirectsConfiguredFastCalls()
            {
                // we setup a request context that returns always the same session id
                var context = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2, FaultAction = "Fault" };

                // calling 3 times should throw an exception
                target.OnActionExecuting(context);
                Assert.IsNotInstanceOfType(context.Result, typeof(RedirectResult));

                target.OnActionExecuting(context);
                target.OnActionExecuting(context);

                Assert.IsInstanceOfType(context.Result, typeof(RedirectResult));
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
                var context = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target1 = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };
                var target2 = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };

                // calling 3 times should throw an exception
                target1.OnActionExecuting(context);
                target2.OnActionExecuting(context);
                target1.OnActionExecuting(context);
            }

            /// <summary>
            /// Tests whether the attribute blocks the configured fast calls by throwing exceptions.
            /// </summary>
            [TestMethod]
            public void NotBlocksLessThanConfiguredFastCalls()
            {
                var context = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };

                // calling 2 times should NOT throw an exception
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);

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
                var context = CreateRequestContext();

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute
                                 {
                                     RequestsPerSecondAndClient = 2,
                                     MaxRetentionTimeOfStatistics = 50,
                                 };

                // calling 2 times should NOT throw an exception
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);

                await Task.Delay(100);

                // calling 2 times should NOT throw an exception
                target.OnActionExecuting(context);
                target.OnActionExecuting(context);

                Assert.IsNotNull(target);
            }

            /// <summary>
            /// Creates a new request context that returns a distinct, but always the same, session id.
            /// </summary>
            /// <returns> The <see cref="Mock"/> object for the request context. </returns>
            private static ActionExecutingContext CreateRequestContext()
            {
                return CreateRequestContext(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
            }

            /// <summary>
            /// Creates a new request context that returns a distinct, but always the same, session id.
            /// </summary>
            /// <param name="sessionId"> The session Id. </param>
            /// <param name="clientIP"> The client IP. </param>
            /// <returns> The <see cref="Mock"/> object for the request context.  </returns>
            private static ActionExecutingContext CreateRequestContext(string sessionId, string clientIP)
            {
                // we setup a request context that returns always the same session id
                var sessionState = new Mock<HttpSessionStateBase>();
                sessionState.Setup(x => x.SessionID).Returns(sessionId);

                var requestBase = new Mock<HttpRequestBase>();
                requestBase.Setup(x => x.UserHostAddress).Returns(clientIP);
                requestBase.Setup(x => x.ApplicationPath).Returns("/");

                var response = new Mock<HttpResponseBase>();
                response.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);

                var httpContext = new Moq.Mock<HttpContextBase>();
                httpContext.Setup(x => x.Session).Returns(sessionState.Object);
                httpContext.Setup(x => x.Request).Returns(requestBase.Object);
                httpContext.Setup(x => x.Response).Returns(response.Object);

                var requestContext = new Moq.Mock<RequestContext>();
                requestContext.Setup(x => x.HttpContext).Returns(httpContext.Object);
                requestContext.Setup(x => x.RouteData).Returns(new RouteData());

                var controller = new Mock<Controller>();
                var routeCollection = new RouteCollection
                                          {
                                              new Route("{controller}/{action}/{id}", new MvcRouteHandler())
                                                  {
                                                      Defaults = CreateRouteValueDictionary(new { controller = "Home", action = "Index", id = UrlParameter.Optional }), 
                                                      Constraints = CreateRouteValueDictionary(null), 
                                                      DataTokens = new RouteValueDictionary()
                                                  }
                                          };
                
                controller.Object.Url = new UrlHelper(requestContext.Object, routeCollection);

                return new ActionExecutingContext
                           {
                               RequestContext = requestContext.Object,
                               Controller = controller.Object,
                           };
            }

            /// <summary>
            /// Creates a route value dictionary.
            /// </summary>
            /// <param name="values"> The values. </param>
            /// <returns> The <see cref="RouteValueDictionary"/>. </returns>
            private static RouteValueDictionary CreateRouteValueDictionary(object values)
            {
                var dictionary = values as IDictionary<string, object>;
                return dictionary != null ? new RouteValueDictionary(dictionary) : new RouteValueDictionary(values);
            }
        }
    }
}
