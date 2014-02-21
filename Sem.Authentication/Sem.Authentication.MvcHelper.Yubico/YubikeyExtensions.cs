// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyExtensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The yubikey extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;

    using Sem.Authentication.AppInfrastructure;
    using Sem.Authentication.MvcHelper.AppInfrastructure;

    /// <summary>
    /// HTML helper extensions for YUBIKEY-functionality.
    /// </summary>
    public static class YubikeyExtensions
    {
        /// <summary>
        /// The YUBIKEY input box markup.
        /// </summary>
        private const string YubikeyInputBox = "<input class=\"yubikeyInput\" id=\"yubiKey\" name=\"yubiKey\" text=\"\"/>";

        /// <summary>
        /// Renders an input tag for the YUBIKEY OTP and an image tag as a "caption".
        /// </summary>
        /// <param name="htmlHelper"> The htmlHelper. </param>
        /// <param name="targetAction"> The target action or target url. If you want a full URL to use, make sure you did add the protocol (https/http). </param>
        /// <returns> The <see cref="IHtmlString"/>. </returns>
        [ExcludeFromCodeCoverage]
        public static IHtmlString YubikeyInput(this HtmlHelper htmlHelper, string targetAction)
        {
            return YubikeyInput(htmlHelper, targetAction, YubikeyConfiguration.DeserializeConfiguration());
        }

        /// <summary>
        /// Renders an input tag for the YUBIKEY OTP and an image tag as a "caption".
        /// </summary>
        /// <param name="htmlHelper"> The htmlHelper. </param>
        /// <param name="targetAction"> The target action or target url. If you want a full URL to use, make sure you did add the protocol (https/http). </param>
        /// <param name="configuration"> The configuration to validate. </param>
        /// <returns> The <see cref="IHtmlString"/>. </returns>
        public static IHtmlString YubikeyInput(this HtmlHelper htmlHelper, string targetAction, ConfigurationBase configuration)
        {
            configuration.EnsureCorrectConfiguration();
            targetAction.ArgumentMustNotBeNull("targetAction");

            if (!targetAction.Contains(":"))
            {
                htmlHelper.ArgumentMustNotBeNull("htmlHelper");
                var controller = (System.Web.Mvc.Controller)htmlHelper.ViewContext.Controller;
                targetAction = controller.Url.Action(targetAction);
            }

            var imgUrl = targetAction + "?42FE943EC8A64735A978D1F81D5FFD00";
            return new HtmlString("<img class=\"yubikeyCaption\" alt=\"YubiKey: \" id=\"yubiKeyImage\" src=\"" + imgUrl + "\" />" + YubikeyInputBox);
        }

        /// <summary>
        /// Renders an input tag for the YUBIKEY OTP and a SPAN tag as a "caption".
        /// </summary>
        /// <param name="htmlHelper"> The htmlHelper. </param>
        /// <returns> The <see cref="IHtmlString"/>. </returns>
        [ExcludeFromCodeCoverage]
        public static IHtmlString YubikeyInput(this HtmlHelper htmlHelper)
        {
            return YubikeyInput(htmlHelper, YubikeyConfiguration.DeserializeConfiguration());
        }

        /// <summary>
        /// Renders an input tag for the YUBIKEY OTP and a SPAN tag as a "caption".
        /// </summary>
        /// <param name="htmlHelper"> The htmlHelper. </param>
        /// <param name="configuration"> The configuration to validate. </param>
        /// <returns> The <see cref="IHtmlString"/>. </returns>
        public static IHtmlString YubikeyInput(this HtmlHelper htmlHelper, ConfigurationBase configuration)
        {
            configuration.EnsureCorrectConfiguration();
            return new HtmlString("<span class=\"yubikeyCaption\" >YubiKey: <span/>" + YubikeyInputBox);
        }
    }
}