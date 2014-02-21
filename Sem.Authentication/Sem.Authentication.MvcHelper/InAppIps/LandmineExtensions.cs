// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LandmineExtensions.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The landmine extensions enables you to add a land mine into the view. This must be used in conjunction with
//   the <see cref="LandmineAttribute" /> in order to work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.InAppIps
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The landmine extensions enables you to add a land mine into the view. This must be used in conjunction with
    /// the <see cref="LandmineMvcAttribute"/> in order to work.
    /// </summary>
    public static class LandmineExtensions
    {
        private static string StyleName = "hidden";

        /// <summary>
        /// Generates a special input tag that contains some predefines value an attacker may try to change in order to 
        /// "hack" your application. As soon as the content of this value has been changed to an unexpected value, the
        /// user will be logged out for a specific time.
        /// </summary>
        /// <param name="htmlHelper"> The html helper. </param>
        /// <param name="forbiddenFieldNames"> The forbidden field names are the field names that might appear on the form. 
        /// This will prevent the method to generate a land mine that might interfere with the values you need for normal 
        /// request processing. </param>
        /// <returns> The <see cref="IHtmlString"/>. </returns>
        [ExcludeFromCodeCoverage]
        public static IHtmlString Landmine(this HtmlHelper htmlHelper, params string[] forbiddenFieldNames)
        {
            var fieldName = GenerateFieldName(forbiddenFieldNames);
            var fieldValue = GenerateFieldValue(fieldName);
            return new HtmlString(string.Format("<input type=\"hidden\" style=\"{0}\" name=\"{1}\" value=\"{2}\" />", StyleName, fieldName, fieldValue));
        }

        /// <summary>
        /// Generates an expected value. The <see cref="LandmineMvcAttribute"/> will be able to recognize this value as "ok".
        /// </summary>
        /// <param name="fieldName"> The field name. </param>
        /// <returns> The <see cref="string"/>. </returns>
        private static string GenerateFieldValue(string fieldName)
        {
            // the value is currently just a constant - we should generate the value from the field name so when 
            // varying the field name, also the value changes. The only thing we should keep in mind is: this 
            // should make a human think that this field might be a good candidate to play with when trying to
            // hack the page. One funny thing may be <input type="hidden" name="userRoleId" value="57" /> that might
            // let the intruder think she/he can play with the access rights by altering the value.
            return "8008";
        }

        /// <summary>
        /// Generates a field name for the land mine.
        /// </summary>
        /// <param name="forbiddenFieldNames"> The forbidden field names. </param>
        /// <returns> The <see cref="string"/>. </returns>
        private static string GenerateFieldName(string[] forbiddenFieldNames)
        {
            return "Landmine";
        }
    }
}
