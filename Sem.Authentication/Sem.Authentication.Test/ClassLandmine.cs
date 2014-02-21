using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem.Authentication.Test
{
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Caching;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Sem.Authentication.InAppIps;

    public static class ClassLandmine
    {
        [TestClass]
        public class Ctor
        {
            [TestMethod]
            public void SetsExpectedDefaults()
            {
                var target = new Landmine();
                Assert.AreEqual("Landmine", target.LandmineName);
                Assert.AreEqual("8008", target.ExpectedValue);
                Assert.AreEqual(120, target.Seconds);
            }
        }

        [TestClass]
        public class RequestGate
        {
            [TestMethod]
            public void ReturnsTrueIfClientIdIsNull()
            {
                var target = new Landmine();
                var result = target.RequestGate(null, null);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsFalseIfCacheContainsLandmineTag()
            {
                HttpRuntime.Cache.Add(
                    "Landmined-B08B9F4B94404FFFAB41E244C69932F7",
                    true,
                    null,
                    DateTime.Now.AddSeconds(10),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.Low,
                    null);

                var target = new Landmine();
                var result = target.RequestGate(null, null);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsTrueIfExpectedValueFoundInForm()
            {
                var requestMock = new Mock<HttpRequestBase>();
                requestMock.Setup(x => x.Form).Returns(new NameValueCollection { { "Landmine", "8008" }, });
                var target = new Landmine { RequestArea = RequestArea.Form };
                var result = target.RequestGate("D3BCEC0D7E724E9FBF8042FAC0DADB0C", requestMock.Object);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsFalseIfExpectedValueNotFoundInForm()
            {
                var requestMock = new Mock<HttpRequestBase>();
                requestMock.Setup(x => x.Form).Returns(new NameValueCollection { { "Landmine", "100" }, });
                var target = new Landmine { RequestArea = RequestArea.Form };
                var result = target.RequestGate("0BDFD8A29F084D5492B0CF2E717E7195", requestMock.Object);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsTrueIfExpectedValueFoundInHeaders()
            {
                var requestMock = new Mock<HttpRequestBase>();
                requestMock.Setup(x => x.Headers).Returns(new NameValueCollection { { "Landmine", "8008" }, });
                var target = new Landmine { RequestArea = RequestArea.Header };
                var result = target.RequestGate("D3BCEC0D7E724E9FBF8042FAC0DADB0C", requestMock.Object);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsFalseIfExpectedValueNotFoundInHeaders()
            {
                var requestMock = new Mock<HttpRequestBase>();
                requestMock.Setup(x => x.Headers).Returns(new NameValueCollection { { "Landmine", "100" }, });
                var target = new Landmine { RequestArea = RequestArea.Header };
                var result = target.RequestGate("0BDFD8A29F084D5492B0CF2E717E7195", requestMock.Object);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsTrueIfExpectedValueFoundInQueryString()
            {
                var requestMock = new Mock<HttpRequestBase>();
                requestMock.Setup(x => x.QueryString).Returns(new NameValueCollection { { "Landmine", "8008" }, });
                var target = new Landmine { RequestArea = RequestArea.QueryString };
                var result = target.RequestGate("51CF66EC32974C40A1F54081CC4D53F7", requestMock.Object);
                Assert.IsTrue(result);
            }

            [TestMethod]
            public void ReturnsFalseIfExpectedValueNotFoundInQueryString()
            {
                var requestMock = new Mock<HttpRequestBase>();
                requestMock.Setup(x => x.QueryString).Returns(new NameValueCollection { { "Landmine", "100" }, });
                var target = new Landmine { RequestArea = RequestArea.QueryString };
                var result = target.RequestGate("0CB0F4EFC88143D7AA4E332B414A91D2", requestMock.Object);
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void ReturnsFalseIfExpectedValueNotEmptyFoundInUnknown()
            {
                var requestMock = new Mock<HttpRequestBase>();
                var target = new Landmine { RequestArea = (RequestArea)Enum.ToObject(typeof(RequestArea), -1) };
                var result = target.RequestGate("6AC9EB3D5C814D728E2D4C8F49C4F480", requestMock.Object);
                Assert.IsFalse(result);
            }
        }
    }
}
