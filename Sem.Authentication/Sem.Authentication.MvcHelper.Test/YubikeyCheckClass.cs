namespace Sem.Authentication.MvcHelper.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.Yubico;

    public static class YubikeyCheckClass
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            [ExcludeFromCodeCoverage]
            public void NullConfigurationThrowsWhenInstance()
            {
                var target = new YubikeyCheck(null) { ImageOnly = true };
                target.OnActionExecuting(MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/?42FE943EC8A64735A978D1F81D5FFD00", UriKind.Absolute)));
            }
        }

        [TestClass]
        public class OnActionExecuting
        {
            [TestMethod]
            public void DeliversImageForCorrectRequest()
            {
                var target = new YubikeyCheck(new YubikeyConfiguration()) { ImageOnly = true };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/?42FE943EC8A64735A978D1F81D5FFD00", UriKind.Absolute));
                
                target.OnActionExecuting(context);

                var fileResult = context.Result as FileResult;
                Assert.IsNotNull(fileResult);
                Assert.AreEqual("image/png", fileResult.ContentType);
            }
        }
    }
}
