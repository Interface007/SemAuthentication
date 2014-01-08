// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassYubikeyExtensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ClassYubikeyExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Authentication.MvcHelper.Test;

    /// <summary>
    /// Tests for the class <see cref="YubikeyExtensions"/>.
    /// </summary>
    public static class ClassYubikeyExtensions
    {
        /// <summary>
        /// Tests the method <see cref="YubikeyExtensions.YubikeyInput(System.Web.Mvc.HtmlHelper,string)"/>.
        /// </summary>
        [TestClass]
        public class YubikeyInput
        {
            /// <summary>
            /// Validates that it renders the expected html for an input box without an image.
            /// </summary>
            [TestMethod]
            public void RendersExpectedInputBox()
            {
                const string Expected = "<span class=\"yubikeyCaption\" >YubiKey: <span/><input class=\"yubikeyInput\" id=\"yubiKey\" name=\"yubiKey\" text=\"\"/>";
                var result = YubikeyExtensions.YubikeyInput(null, new YubikeyConfiguration());
                Assert.AreEqual(Expected, result.ToHtmlString());
            }

            /// <summary>
            /// Validates that it renders the expected html for the input box with an image from a specific URL url.
            /// </summary>
            [TestMethod]
            public void RendersExpectedInputBoxWithImageForUrl()
            {
                const string Expected = "<img class=\"yubikeyCaption\" alt=\"YubiKey: \" id=\"yubiKeyImage\" src=\"http://target/action?42FE943EC8A64735A978D1F81D5FFD00\" /><input class=\"yubikeyInput\" id=\"yubiKey\" name=\"yubiKey\" text=\"\"/>";
                var result = YubikeyExtensions.YubikeyInput(null, "http://target/action", new YubikeyConfiguration());
                Assert.AreEqual(Expected, result.ToHtmlString());
            }

            /// <summary>
            /// Validates that it renders the expected html for the input box with an image for specific action.
            /// </summary>
            [TestMethod]
            public void RendersExpectedInputBoxWithImageForAction()
            {
                const string Expected = "<img class=\"yubikeyCaption\" alt=\"YubiKey: \" id=\"yubiKeyImage\" src=\"/Home/targetaction?42FE943EC8A64735A978D1F81D5FFD00\" /><input class=\"yubikeyInput\" id=\"yubiKey\" name=\"yubiKey\" text=\"\"/>";
                var result = MvcTestBase.CreateHtmlHelper().YubikeyInput("targetaction", new YubikeyConfiguration());
                Assert.AreEqual(Expected, result.ToHtmlString());
            }
        }
    }
}
