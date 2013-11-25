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
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.Exceptions;

    using YubicoDotNetClient;

    /// <summary>
    /// MVC filter attribute to add a request filter that does check for a YUBIKEY and its validity.
    /// </summary>
    public class YubikeyCheck : AuthenticationCheck
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
        /// Gets the name of the image to be returned for this authenticator.
        /// </summary>
        protected override string ImageName
        {
            get
            {
                return "yubikey-finger.png";
            }
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void InternalAuthenticationCheck(ActionExecutingContext filterContext)
        {
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

            IYubicoResponse response;
            try
            {
                response = client.Verify(otp);
            }
            catch (Exception ex)
            {
                throw new YubikeyInvalidResponseException(YubicoResponseStatus.BackendError, ex);
            }

            if (response == null)
            {
                throw new YubikeyNullResponseException();
            }

            var user = filterContext.HttpContext.User;
            var users = this.configuration.Users;
            if (response.Status == YubicoResponseStatus.Ok 
             && response.PublicId == users.FirstOrDefault(x => x.Name == user.Identity.Name).ExternalId)
            {
                return;
            }

            throw new YubikeyInvalidResponseException(response.Status);
        }
    }
}
