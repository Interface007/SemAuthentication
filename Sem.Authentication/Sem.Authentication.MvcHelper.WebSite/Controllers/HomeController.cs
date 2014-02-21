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
    using System;
    using System.Web.Mvc;

    using Sem.Authentication.InAppIps.Processing;
    using Sem.Authentication.MvcHelper.InAppIps;
    using Sem.Authentication.MvcHelper.Yubico;

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
        public ActionResult About()
        {
            ViewBag.Message = "Sample Application.";
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
            ViewBag.Message = "Authentication succeeded!";
            return this.View();
        }

        /// <summary>
        /// The "invalid key" page to be shown if the key/token is not valid.
        /// </summary>
        /// <returns>The action to show the associated view.</returns>
        public ActionResult InvalidKey()
        {
            ViewBag.Message = "InvalidKey!";
            return this.View();
        }

        /// <summary>
        /// The "only one request per second" page that will redirect to the action <see cref="Fault"/>
        /// as soon as a user does request more than 1 action calls per second.
        /// <see cref="FastRequestsProtectionAttribute"/> for more information about this protection.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [FastRequestsProtection(RequestsPerSecondAndClient = 1, FaultAction = "Fault")]
        public ActionResult OnlyOneRequestPerSecond()
        {
            ViewBag.Message = "One Request per Second Only!";
            return this.View(new Tuple<string>(string.Empty));
        }

        /// <summary>
        /// The "min one second per request" page that will redirect to the action <see cref="Fault"/>
        /// as soon as a user does request more than 1 action calls per second.
        /// <see cref="FastRequestsProtectionAttribute"/> for more information about this protection.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [MinimumRequestTimeDistanceAttribute(Seconds = 1, Message = "Fault")]
        public ActionResult MinOneSecBetweenRequests()
        {
            ViewBag.Message = "One Request per Second Only!";
            return this.View(new Tuple<string>(string.Empty));
        }

        /// <summary>
        /// The fault handling action. The parameters are all optional.
        /// </summary>
        /// <param name="faultSource"> The fault source. </param>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        public ActionResult Fault(string faultSource = "")
        {
            return this.View(new Tuple<string>(faultSource));
        }

        /// <summary>
        /// The land mine protected view that simply shows the success of an action.
        /// </summary>
        /// <returns> The <see cref="ActionResult"/>. </returns>
        [LandmineMvc(typeof(UserHostExtractor))]
        public ActionResult LandMineProtected()
        {
            return this.View();
        }
    }
}