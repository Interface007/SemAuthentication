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
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.Exceptions;

    /// <summary>
    /// The authentication check base class.
    /// </summary>
    public abstract class AuthenticationCheck : ActionFilterAttribute
    {
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
            try
            {
                this.InternalAuthenticationCheck(filterContext);
            }
            catch (AuthenticationFilterException ex)
            {
                //// TODO: implement logging of exceptions

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
    }
}