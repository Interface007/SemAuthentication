// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationCheckAttribute.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the AuthenticationCheckAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.WebApiHelper.AppInfrastructure
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Sem.Authentication.AppInfrastructure;

    /// <summary>
    /// The authentication check base class.
    /// </summary>
    public abstract class AuthenticationCheckAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ConfigurationBase configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationCheckAttribute"/> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        protected AuthenticationCheckAttribute(ConfigurationBase configuration)
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
        /// Gets or sets the audit implementation.
        /// </summary>
        protected ISemAudit Audit { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            try
            {
                // filterContext.HttpContext is probelmatic to test, because instead of simply letting it NULL, the
                // filter context does create an internal class instance that cannot be easily tested for from outside
                if (filterContext == null || filterContext.Request == null)
                {
                    throw new ArgumentNullException("filterContext", "the parameter must contain a full initialized context object includeing the property path filterContext.Request.Properties[MS_HttpContext].");
                }

                var httpContextBase = filterContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
                if (httpContextBase == null)
                {
                    throw new NullReferenceException("the parameter must contain a full initialized context object includeing the property path filterContext.Request.Properties[MS_HttpContext].");
                }

                var url = httpContextBase.Request.Url;
                if (url != null && url.Query.Contains("42FE943EC8A64735A978D1F81D5FFD00"))
                {
                    var assembly = this.GetType().Assembly;

                    // don't need to dispose this stream here, because the FileStreamResult will do that for us while writing to the response stream
                    // using the types namespace instead of a string will prevent issues with rename refacoring
                    var stream = assembly.GetManifestResourceStream(this.GetType().Namespace + ".Content." + this.ImageName);
                    filterContext.Response = this.StreamResult(stream, "image/png");
                    return;
                }

                if (this.ImageOnly)
                {
                    // since we tagged the attribute to ONLY provide the image, but NO validation, we can exit here
                    return;
                }

                this.InternalAuthenticationCheck(httpContextBase);
                this.AuditSuccess(httpContextBase);
            }
            catch (Exception ex)
            {
                this.Log(ex);
                this.AuditFailure(filterContext, ex);

                // if we don't know where to redirect to, we simply re-throw the exception
                if (string.IsNullOrEmpty(this.InvalidKeyAction) || filterContext == null)
                {
                    throw;
                }

                //// TODO: we should serialize the exception and the current request parameters/form data to be able to retry the request.

                // we have a configuration for the "error page action", so we redirect to there.
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Moved);
                filterContext.Response.Headers.Location = new Uri(this.InvalidKeyAction);
            }
        }

        private HttpResponseMessage StreamResult(Stream stream, string contentType)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                var httpResponseMessage = new HttpResponseMessage
                                              {
                                                  Content = new ByteArrayContent(memoryStream.ToArray()),
                                                  StatusCode = HttpStatusCode.OK
                                              };

                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return httpResponseMessage;
            }
        }

        /// <summary>
        /// The internal authentication check - to be implemented by the sub class.
        /// </summary>
        /// <param name="filterContext"> The filter context. </param>
        protected abstract void InternalAuthenticationCheck(HttpContextBase filterContext);

        /// <summary>
        /// Creates a logger and logs the exception <paramref name="ex"/>.
        /// </summary>
        /// <param name="ex"> The exception to be logged. </param>
        protected void Log(Exception ex)
        {
            var logger = this.Logger ?? (this.Logger = this.CreateLogger());
            if (logger != null)
            {
                logger.Log(ex);
            }
        }

        /// <summary>
        /// Creates an audit writer and logs the exception.
        /// </summary>
        /// <param name="filterContext">The current filter context to get the user and the action.</param>
        /// <param name="exception"> The exception. </param>
        protected void AuditFailure(HttpActionContext filterContext, Exception exception)
        {
            var audit = this.Audit ?? (this.Audit = this.CreateAudit());
            if (audit == null)
            {
                return;
            }

            filterContext.ArgumentMustNotBeNull("filterContext");
            
            // todo: find the current user name
            audit.AuthenticationCheckFailed(new AuditInfo<string>("user not known", exception.Message));
        }

        /// <summary>
        /// Creates an audit writer and logs the success.
        /// </summary>
        /// <param name="filterContext">The current filter context to get the user and the action.</param>
        protected void AuditSuccess(HttpContextBase filterContext)
        {
            var audit = this.Audit ?? (this.Audit = this.CreateAudit());
            if (audit == null)
            {
                return;
            }

            filterContext.ArgumentMustNotBeNull("filterContext");
            audit.AuthenticationCheckSucceeded(new AuditInfo<Exception>(filterContext));
        }

        /// <summary>
        /// The create type instance.
        /// </summary>
        /// <param name="typeConfiguration"> The type configuration. </param>
        /// <typeparam name="T"> The type to return. </typeparam>
        /// <returns> The new instance of <see cref="T"/>. </returns>
        private static T CreateTypeInstance<T>(TypeConfiguration typeConfiguration)
            where T : class
        {
            if (typeConfiguration == null || string.IsNullOrEmpty(typeConfiguration.TypeName))
            {
                return null;
            }

            var loggerType = Type.GetType(typeConfiguration.TypeName);
            if (loggerType == null)
            {
                return null;
            }

            return (T)Activator.CreateInstance(loggerType);
        }

        /// <summary>
        /// Creates an audit writer instance according to the type configured in the file <c>YubiKey.xml</c>.
        /// </summary>
        /// <returns> The <see cref="ISemAudit"/> implementation. </returns>
        private ISemAudit CreateAudit()
        {
            return this.configuration == null ? null : CreateTypeInstance<ISemAudit>(this.configuration.Audit);
        }

        /// <summary>
        /// Creates a logger instance according to the type configured in the file <c>YubiKey.xml</c>.
        /// </summary>
        /// <returns> The <see cref="ISemAuthLogger"/> implementation. </returns>
        private ISemAuthLogger CreateLogger()
        {
            return this.configuration == null ? null : CreateTypeInstance<ISemAuthLogger>(this.configuration.Logger);
        }
    }
}