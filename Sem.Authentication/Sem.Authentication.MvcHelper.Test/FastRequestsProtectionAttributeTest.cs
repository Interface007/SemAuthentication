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
                Assert.Inconclusive("we need a way to test this.");
            }
        }

        [TestClass]
        public class Interfaces
        {
            [TestMethod]
            public void ImplementsActionFilterAttribute()
            {
                var target = new FastRequestsProtectionAttribute();
                Assert.IsInstanceOfType(target, typeof(ActionFilterAttribute));
            }
        }

        [TestClass]
        public class OnActionExecuting
        {
            [TestMethod]
            [ExpectedException(typeof(HttpException))]
            public void BlocksConfiguredFastCalls()
            {
                // we setup a request context that returns always the same session id
                var session = new Mock<HttpSessionStateBase>();
                session.Setup(x => x.SessionID).Returns("hello");
                var hcontext = new Moq.Mock<HttpContextBase>();
                hcontext.Setup(x => x.Session).Returns(session.Object);
                var rcontext = new Moq.Mock<RequestContext>();
                rcontext.Setup(x => x.HttpContext).Returns(hcontext.Object);

                // we want to allow max 2 requests in one second
                var target = new FastRequestsProtectionAttribute { RequestsPerSecondAndClient = 2 };
                
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
                target.OnActionExecuting(new ActionExecutingContext { RequestContext = rcontext.Object });
            }
        }
    }
}
