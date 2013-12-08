// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationCheck.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the AuthenticationCheck type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// The authentication check base class.
    /// </summary>
    public abstract class AuthenticationCheck : ActionFilterAttribute
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ConfigurationBase configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCheck"/> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        protected AuthenticationCheck(ConfigurationBase configuration)
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
        /// Gets or sets a value indicating whether the token is known, but the current user identity does not need to match.
        /// Setting this value to "true" might be useful in scenarios where only user logs in and a second must provide a token
        /// value as a second authenticator.
        /// </summary>
        public bool SkipIdentityNameCheck { get; set; }

        /// <summary>
        /// Gets the name of the image to be returned for this authenticator.
        /// </summary>
        protected abstract string ImageName { get; }

        /// <summary>
        /// Gets or sets the logger implementation.
        /// </summary>
        protected ISemAuthLogger Logger { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var url = filterContext.HttpContext.Request.Url;
                if (url != null && url.Query.Contains("42FE943EC8A64735A978D1F81D5FFD00"))
                {
                    var assembly = this.GetType().Assembly;

                    // don't need to dispose this stream here, because the FileStreamResult will do that for us while writing to the response stream
                    // using the types namespace instead of a string will prevent issues with rename refacoring
                    var stream = assembly.GetManifestResourceStream(this.GetType().Namespace + ".Content." + this.ImageName);
                    filterContext.Result = new FileStreamResult(stream, "image/png");
                    return;
                }

                if (this.ImageOnly)
                {
                    // since we tagged the attribute to ONLY provide the image, but NO validation, we can exit here
                    return;
                }

                this.InternalAuthenticationCheck(filterContext);
            }
            catch (Exception ex)
            {
                this.Log(ex);

                // if we don't know where to redirect to, we simply re-throw the exception
                if (string.IsNullOrEmpty(this.InvalidKeyAction))
                {
                    throw;
                }

                //// TODO: we should serialize the exception and the current request parameters/form data to be able to retry the request.

                // we have a configuration for the "error page action", so we redirect to there.
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(urlHelper.Action(this.InvalidKeyAction));
            }
        }

        /// <summary>
        /// The internal authentication check - to be implemented by the sub class.
        /// </summary>
        /// <param name="filterContext"> The filter context. </param>
        protected abstract void InternalAuthenticationCheck(ActionExecutingContext filterContext);

        private void Log(Exception ex)
        {
            var logger = this.Logger ?? this.CreateLogger();
            if (logger != null)
            {
                logger.Log(ex);
            }
        }

        private ISemAuthLogger CreateLogger()
        {
            if (string.IsNullOrEmpty(this.configuration.Logger.TypeName))
            {
                return null;
            }

            var loggerType = Type.GetType(this.configuration.Logger.TypeName);
            if (loggerType == null)
            {
                return null;
            } 
            
            return this.Logger = (ISemAuthLogger)Activator.CreateInstance(loggerType);
        }
    }
}