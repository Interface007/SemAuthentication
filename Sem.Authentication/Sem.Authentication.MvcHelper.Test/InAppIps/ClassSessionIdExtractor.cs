// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassSessionIdExtractor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the SessionIdExtractorTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.InAppIps
{
    using System.Web;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.InAppIps.Processing;

    public static class ClassSessionIdExtractor
    {
        [TestClass]
        public class Extract
        {
            [TestMethod]
            public void HandelsNull()
            {
                var target = new SessionIdExtractor();
                var httpContext = new Moq.Mock<HttpContextBase>();
                var result = target.Extract(httpContext.Object);
                Assert.AreEqual(string.Empty, result);
            }
        }
    }
}