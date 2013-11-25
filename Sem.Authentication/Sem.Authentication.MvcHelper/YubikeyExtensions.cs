// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyExtensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The yubikey extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// HTML helper extensions for YUBIKEY-functionality.
    /// </summary>
    public static class YubikeyExtensions
    {
        /// <summary>
        /// Renders an input tag for the YUBIKEY OTP and an image tag as a "caption".
        /// </summary>
        /// <param name="view"> The view. </param>
        /// <param name="targetAction"> The target action. </param>
        /// <returns> The <see cref="IHtmlString"/>. </returns>
        public static IHtmlString YubikeyInput(this HtmlHelper view, string targetAction)
        {
            var imgUrl = targetAction + "?42FE943EC8A64735A978D1F81D5FFD00";

            var text = "<img alt=\"YubiKey: \" id=\"yubiKeyImage\" src=\"" + imgUrl + "\" />" +
                       "<input id=\"yubiKey\" name=\"yubiKey\" text=\"\"/>";

            return new HtmlString(text);
        }
    }
}