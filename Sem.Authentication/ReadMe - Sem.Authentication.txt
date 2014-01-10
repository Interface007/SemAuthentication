This library provides some "tools" for dealing with authentication like MVC 
attributes to add another authentication factor when accessing specific MVC 
actions.

How can I start using it?

Use the following HTML inside a view to create a form that includes a YUBICO
OTP inside the post request:

    <div class="col-md-4">
        <h2>Yubico Authentication</h2>
        <p>
            This MVC action is protected by an
        </p>
        <form method="POST" action="@Url.Action("YubikeyProtected")">
            <p>
                @* The input box you might omit the action in this call, but then you will only get a span with the input element instead of the image and the input element *@
                @Html.YubikeyInput("YubikeyProtected")
                <input type="submit" class="btn btn-default" value="try it &raquo;" />
            </p>
        </form>
    </div>

The following code implements a controller action that needs a YUBICO OTP
to be included in the request. If the YUBICO OTP is not included, the
request will be forwarded to the action "InvalidKey".

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

This action will requeire the user to pause 1 second between two requests:

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
