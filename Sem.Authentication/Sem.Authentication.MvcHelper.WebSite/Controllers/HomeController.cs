// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the HomeController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.WebSite.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The index action to view the start page.
        /// </summary>
        /// <returns>The action to show the associated view.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// The about page.
        /// </summary>
        /// <returns>The action to show the associated view.</returns>
        [FastRequestsProtection(RequestsPerSecondAndClient = 2)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return this.View();
        }

        /// <summary>
        /// The contact page.
        /// </summary>
        /// <returns>The action to show the associated view.</returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return this.View();
        }

        /// <summary>
        /// In this case we simply check, if the key is "valid", not if the user name matches the current user identity.
        /// </summary>
        /// <returns>The action to show the associated view.</returns>
        [YubikeyCheck(SkipIdentityNameCheck = true, InvalidKeyAction = "InvalidKey")]
        public ActionResult YubikeyProtected()
        {
            return this.View();
        }

        /// <summary>
        /// The "invalid key" page to be shown if the key/token is not valid.
        /// </summary>
        /// <returns>The action to show the associated view.</returns>
        public ActionResult InvalidKey()
        {
            return this.View();
        }
    }
}