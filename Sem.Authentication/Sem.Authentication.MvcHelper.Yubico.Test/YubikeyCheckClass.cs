// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyCheckClass.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyCheckClass type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web.Mvc;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Sem.Authentication.MvcHelper.AppInfrastructure;
    using Sem.Authentication.MvcHelper.Test;
    using Sem.Authentication.MvcHelper.Yubico;
    using Sem.Authentication.MvcHelper.Yubico.Client;
    using Sem.Authentication.MvcHelper.Yubico.Exceptions;

    using YubicoDotNetClient;

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
                var target = new YubikeyCheck(null, new YubicoClientAbstraction()) { ImageOnly = true };
                target.OnActionExecuting(MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/?42FE943EC8A64735A978D1F81D5FFD00", UriKind.Absolute)));
            }
        }

        [TestClass]
        public class OnActionExecuting
        {
            [TestMethod]
            public void DeliversImageForCorrectRequest()
            {
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/?42FE943EC8A64735A978D1F81D5FFD00", UriKind.Absolute));

                var target = new YubikeyCheck(new YubikeyConfiguration(), new YubicoClientAbstraction()) { ImageOnly = true };
                target.OnActionExecuting(context);

                var fileResult = context.Result as FileResult;
                Assert.IsNotNull(fileResult);
                Assert.AreEqual("image/png", fileResult.ContentType);
            }

            [TestMethod]
            public void DoesNotInterceptNonImageRequestsWhenImageOnly()
            {
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute));

                var target = new YubikeyCheck(new YubikeyConfiguration(), new YubicoClientAbstraction()) { ImageOnly = true };
                target.OnActionExecuting(context);

                Assert.IsNull(context.Result);
            }

            [TestMethod]
            public void CallsYubicoClientForAuthentiaction()
            {
                var client = SetupClient(YubicoResponseStatus.Ok, "some id");
                var configuration = new YubikeyConfiguration { Users = new List<UserMapping> { new UserMapping("some name", "some id") } };
                var formCollection = new NameValueCollection { { "yubiKey", "some id" } };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute), null, null, formCollection);

                var target = new YubikeyCheck(configuration, client.Object) { SkipIdentityNameCheck = true, };
                target.OnActionExecuting(context);

                client.Verify();
            }

            [TestMethod]
            [ExpectedException(typeof(YubikeyInvalidResponseException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsYubikeyInvalidResponseExceptionWhenResponseNotOk()
            {
                var client = SetupClient(YubicoResponseStatus.BadOtp, "some id");
                var configuration = new YubikeyConfiguration { Users = new List<UserMapping> { new UserMapping("some name", "some id") } };
                var formCollection = new NameValueCollection { { "yubiKey", "some id" } };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute), null, null, formCollection);

                var target = new YubikeyCheck(configuration, client.Object) { SkipIdentityNameCheck = true, };
                target.OnActionExecuting(context);
            }

            [TestMethod]
            [ExpectedException(typeof(YubikeyInvalidResponseException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsYubikeyInvalidResponseExceptionWhenUnknownId()
            {
                var client = SetupClient(YubicoResponseStatus.Ok, "some other id");
                var configuration = new YubikeyConfiguration { Users = new List<UserMapping> { new UserMapping("some name", "some id") } };
                var formCollection = new NameValueCollection { { "yubiKey", "some id" } };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute), null, null, formCollection);

                var target = new YubikeyCheck(configuration, client.Object) { SkipIdentityNameCheck = true, };
                target.OnActionExecuting(context);
            }

            [TestMethod]
            [ExpectedException(typeof(YubikeyNotPresentException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsYubikeyNotPresentExceptionWhenKeyNotPresent()
            {
                var client = SetupClient(YubicoResponseStatus.Ok, "some other id");
                var configuration = new YubikeyConfiguration { Users = new List<UserMapping> { new UserMapping("some name", "some id") } };
                var formCollection = new NameValueCollection { { "yubiKez", "some id" } };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute), null, null, formCollection);

                var target = new YubikeyCheck(configuration, client.Object) { SkipIdentityNameCheck = true, };
                target.OnActionExecuting(context);
            }

            [TestMethod]
            [ExpectedException(typeof(YubikeyInvalidResponseException))]
            [ExcludeFromCodeCoverage]
            public void ThrowsYubikeyInvalidResponseExceptionWhenExceptionWhileClientCall()
            {
                var response = new Mock<IYubicoResponse>();
                response.SetupGet(x => x.Status).Returns(YubicoResponseStatus.Ok);
                response.SetupGet(x => x.PublicId).Returns("some other id");

                var client = new Mock<IYubicoClient>();
                client.Setup(x => x.Verify(It.IsAny<string>())).Throws<WebException>();
                var configuration = new YubikeyConfiguration { Users = new List<UserMapping> { new UserMapping("some name", "some id") } };
                var formCollection = new NameValueCollection { { "yubiKey", "some id" } };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute), null, null, formCollection);

                var target = new YubikeyCheck(configuration, client.Object) { SkipIdentityNameCheck = true, };
                target.OnActionExecuting(context);
            }

            [TestMethod]
            public void ThrowsNotWhenKnownId()
            {
                var client = SetupClient(YubicoResponseStatus.Ok, "some id");
                var configuration = new YubikeyConfiguration { Users = new List<UserMapping> { new UserMapping("some name", "some id") } };
                var formCollection = new NameValueCollection { { "yubiKey", "some id" } };
                var context = MvcTestBase.CreateRequestContext(new Uri("http://localhost/test/", UriKind.Absolute), null, null, formCollection);

                var target = new YubikeyCheck(configuration, client.Object) { SkipIdentityNameCheck = true, };
                target.OnActionExecuting(context);
            }

            private static Mock<IYubicoClient> SetupClient(YubicoResponseStatus responseStatus, string publicId)
            {
                var response = new Mock<IYubicoResponse>();
                response.SetupGet(x => x.Status).Returns(responseStatus);
                response.SetupGet(x => x.PublicId).Returns(publicId);

                var client = new Mock<IYubicoClient>();
                client.Setup(x => x.Verify(It.IsAny<string>())).Returns(response.Object).Verifiable();
                return client;
            }
        }
    }
}
