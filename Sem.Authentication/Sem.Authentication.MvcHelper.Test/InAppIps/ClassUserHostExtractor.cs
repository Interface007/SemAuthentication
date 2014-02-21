// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassUserHostExtractor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the UserHostExtractorTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.InAppIps
{
    using System.Collections.Specialized;
    using System.Web;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.InAppIps.Processing;

    public static class ClassUserHostExtractor
    {
        [TestClass]
        public class Extract
        {
            [TestMethod]
            public void HandelsNull()
            {
                var target = new UserHostExtractor();
                var result = target.Extract(null);
                Assert.AreEqual(string.Empty, result);
            }

            [TestMethod]
            public void ExtractsUserHostAddress()
            {
                var target = new UserHostExtractor();
                var httpContext = new Moq.Mock<HttpContextBase>();
                var request = new Moq.Mock<HttpRequestBase>();
                
                request.Setup(x => x.UserHostAddress).Returns("10.0.0.1");
                httpContext.Setup(x => x.Request).Returns(request.Object);
                
                var result = target.Extract(httpContext.Object);
                
                Assert.AreEqual("10.0.0.1", result);
            }

            [TestMethod]
            public void HandelsNullRequest()
            {
                var target = new UserHostExtractor();
                var httpContext = new Moq.Mock<HttpContextBase>();
                
                var result = target.Extract(httpContext.Object);
                
                Assert.AreEqual(string.Empty, result);
            }

            [TestMethod]
            public void ExtractsUserHostAddressPlusHeader()
            {
                var target = new UserHostExtractor();
                var httpContext = new Moq.Mock<HttpContextBase>();
                var request = new Moq.Mock<HttpRequestBase>();
                var headers = new NameValueCollection { { "REMOTE_ADDR", "10.0.0.2" } };
                
                request.Setup(x => x.UserHostAddress).Returns("10.0.0.1");
                request.Setup(x => x.Headers).Returns(headers);
                httpContext.Setup(x => x.Request).Returns(request.Object);
                
                var result = target.Extract(httpContext.Object);

                Assert.AreEqual("10.0.0.1-10.0.0.2-", result);
            }

            [TestMethod]
            public void AddsFirstAddressFromForwardedHeader()
            {
                var target = new UserHostExtractor();
                var httpContext = new Moq.Mock<HttpContextBase>();
                var request = new Moq.Mock<HttpRequestBase>();
                var headers = new NameValueCollection
                                  {
                                      { "REMOTE_ADDR", "10.0.0.2" },
                                      { "HTTP_X_FORWARDED_FOR", "10.0.0.3,10.0.0.4" },
                                  };
                
                request.Setup(x => x.UserHostAddress).Returns("10.0.0.1");
                request.Setup(x => x.Headers).Returns(headers);
                httpContext.Setup(x => x.Request).Returns(request.Object);
                
                var result = target.Extract(httpContext.Object);

                Assert.AreEqual("10.0.0.1-10.0.0.2-10.0.0.3", result);
            }
        }
    }
}