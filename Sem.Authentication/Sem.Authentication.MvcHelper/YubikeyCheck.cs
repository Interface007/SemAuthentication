// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyCheck.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyCheck type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System.Linq;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.Exceptions;

    using YubicoDotNetClient;

    /// <summary>
    /// MVC filter attribute to add a request filter that does check for a YUBIKEY and its validity.
    /// </summary>
    public class YubikeyCheck : ActionFilterAttribute
    {
        /// <summary>
        /// The server configuration.
        /// </summary>
        private readonly YubikeyConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyCheck"/> class.
        /// </summary>
        public YubikeyCheck()
            : this(YubikeyConfiguration.DeserializeConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyCheck"/> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public YubikeyCheck(YubikeyConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to process only the image request.
        /// Since this control uses the simple URL without parameters and adds a unique value to compose a
        /// request that returns the YUBIKEY-image, we need to deal with situations in which the action has 
        /// been overloaded and the default action does not need YUBIKEY validation. In such a case you
        /// might set this value to "true" and validation will be skipped.
        /// </summary>
        public bool ImageOnly { get; set; }

        /// <summary>
        /// Gets or sets the action to call for an invalid key.
        /// </summary>
        public string InvalidKeyAction { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;
            if (url != null && url.Query.Contains("42FE943EC8A64735A978D1F81D5FFD00"))
            {
                var assembly = this.GetType().Assembly;

                // don't need to dispose this stream here, because the FileStreamResult will do that for us while writing to the response stream
                // using the types namespace instead of a string will prevent issues with rename refacoring
                var stream = assembly.GetManifestResourceStream(this.GetType().Namespace + ".Content.yubikey-finger.png");
                filterContext.Result = new FileStreamResult(stream, "image/png");
                return;
            }

            if (this.ImageOnly)
            {
                // since we tagged the attribute to ONLY provide the image, but NO validation, we can exit here
                return;
            }

            // to validate, we need the value of the key - have a look if we can find it.
            var parameters = filterContext.HttpContext.Request.Form;
            var containskey = parameters.Keys.OfType<string>().Contains("yubiKey");
            if (!containskey)
            {
                throw new YubikeyNotPresentException();
            }

            var otp = parameters["yubiKey"];

            var server = this.configuration.Server;
            var client = new YubicoClient(server.ClientId);
            client.SetApiKey(server.ApiKey);
            client.SetSync(server.SyncLevel);

            var response = client.Verify(otp);
            if (response == null)
            {
                throw new YubikeyNullResponseException();
            }

            var user = filterContext.HttpContext.User;
            var users = this.configuration.Users;
            if (response.Status == YubicoResponseStatus.Ok && response.PublicId == users.FirstOrDefault(x => x.Name == user.Identity.Name).ExternalId)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.InvalidKeyAction))
            {
                throw new YubikeyInvalidResponseException(response.Status);
            }

            var urlHelper = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(urlHelper.Action(this.InvalidKeyAction));
        }
    }
}
