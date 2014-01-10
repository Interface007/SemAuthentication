// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyCheckAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyCheckAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.AppInfrastructure;
    using Sem.Authentication.MvcHelper.Yubico.Client;
    using Sem.Authentication.MvcHelper.Yubico.Exceptions;

    using YubicoDotNetClient;

    /// <summary>
    /// MVC filter attribute to add a request filter that does check for a YUBIKEY and its validity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class YubikeyCheckAttribute : AuthenticationCheckAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyCheckAttribute"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public YubikeyCheckAttribute()
            : this(YubikeyConfiguration.DeserializeConfiguration(), new YubicoClientAdapter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyCheckAttribute"/> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        /// <param name="client">The implementation of <see cref="IYubicoClient"/> to use.</param>
        public YubikeyCheckAttribute(YubikeyConfiguration configuration, IYubicoClient client)
            : base(configuration)
        {
            this.Configuration = configuration;
            this.Client = client;

            if (configuration == null || configuration.Server == null || client == null)
            {
                return;
            }

            var server = this.Configuration.Server;
            this.Client.ClientId = server.ClientId;
            this.Client.ApiKey = server.ApiKey;
            this.Client.SyncLevel = server.SyncLevel;
        }

        /// <summary>
        /// Gets the server configuration.
        /// </summary>
        public YubikeyConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the client implementation of <see cref="IYubicoClient"/>.
        /// </summary>
        public IYubicoClient Client { get; private set; }

        /// <summary>
        /// Gets the name of the image to be returned for this authenticator.
        /// </summary>
        protected override string ImageName
        {
            get
            {
                this.Configuration.EnsureCorrectConfiguration();
                return "yubikey-finger.png";
            }
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void InternalAuthenticationCheck(ActionExecutingContext filterContext)
        {
            this.Configuration.EnsureCorrectConfiguration();
            filterContext.ArgumentMustNotBeNull("filterContext");

            // to validate, we need the value of the key - have a look if we can find it.
            var parameters = filterContext.HttpContext.Request.Form;
            var containskey = parameters.Keys.OfType<string>().Contains("yubiKey");
            if (!containskey)
            {
                throw new YubikeyNotPresentException();
            }

            var otp = parameters["yubiKey"];

            IYubicoResponse response;
            try
            {
                response = this.Client.Verify(otp);
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
            var users = this.Configuration.Users;

            if (users == null)
            {
                throw new InvalidOperationException("The Users property of the configuration is NULL.");
            }

            // the response must be OK 
            // the name of the current http context identity must match, OR SkipIdentityNameCheck must be enabled
            if (response.Status == YubicoResponseStatus.Ok
             && (this.SkipIdentityNameCheck
                    ? users.Any(x => x.ExternalId == response.PublicId)
                    : users.Where(x => x.ExternalId == response.PublicId).Select(x => x.Name).FirstOrDefault() == user.Identity.Name))
            {
                return;
            }

            throw new YubikeyInvalidResponseException(response.Status);
        }
    }
}