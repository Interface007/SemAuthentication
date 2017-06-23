# SemAuthentication
*Project Description*
{project:description}

The purpose of this project is to provide tools for an MVC application developer to ease some tasks in the field of authentication and authorization.

What problems does it solve?
In web applications you face issues like getting a 2nd factor of authentication for specific actions or preventing the usage of attack tools like havij ([url:http://itsecteam.com/products/havij-advanced-sql-injection/]). This library provides you with easy to use ASP.Net MVC attributes to handle such challenges.

Current feature list:
* Attribute based additional authentication factor for MVC controller actions
The attribute "YubikeyCheck" validates a YubiKey ([url:http://www.yubico.com/]) token string and redirects to a specific page if the token is not valid (list of users that are allowed to authenticate via YubiKey can be configured inside an XML file). If you have a different 2nd factor, you can inherit from an abstract base class “AuthenticationCheck” that does provide some of the “infrastructure” for such a check.
* Attribute based throttling of requests from one client
The attribute "MinimumRequestTimeDistance" allows to set a minimum of time between two requests from a single client for a specific controller action. With the attribute "FastRequestsProtection" you can specify how many requests per second can be issued from one client (this allows the client to issue "n" requests simultaneously, but then the client has to wait). Both attributes will lower the effectiveness of automated attack tools that mostly rely on a mass of requests issued in short time.

! Samples
!! 2nd factor check
Modify an ASP.Net-MVC controller action to require the user to enter a One-Time-Password (in this case via YubiKey). In this sample the only modification has been to add the attribute "YubikeyCheck()" to the method. The attribute property "InvalidKeyAction" does specify the controller action to redirect to in case of the token to not be correct.
{code:c#}
[HttpPost]
[ValidateAntiForgeryToken]
[YubikeyCheck(InvalidKeyAction = "InvalidKey")]
public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] eXperts experts)
{
   ...
{code:c#}
Adding the UI-Elements to render an input box for the One-Time-Password. The only modification of the original view was to add the row "@Html.YubikeyInput(Url.Action("Edit")) <br />". This renders an additional image and an input-box with the name "yubiKey". The value of this input-box will be validated if the attribute "YubikeyCheck" has been added to a controller action.
{code:c#}
<div class="form-group">
    <div class="col-md-offset-2 col-md-10">
        @Html.YubikeyInput(Url.Action("Edit")) <br />
        <input type="submit" value="Save" class="btn btn-default" />
    </div>
</div>
{code:c#}
!! Fast repetitive request protection
Modify an ASP.Net MVC controller action to forward a 2nd request within 1 second to a different action to prevent database exploration with attack tools. In the following code the attribute redirects a 2nd request from the same client to another action. This way you cannot use tool against this action that rely on quick repetitive execution of this action.
{code:c#}
[MinimumRequestTimeDistanceAttribute(Seconds = 1, Message = "Fault")]
public ActionResult MinOneSecBetweenRequests()
{
    ViewBag.Message = "One Request per Second Only!";
    return this.View(new Tuple<string>(string.Empty));
}
public ActionResult Fault(string faultSource = "")
{
    return this.View(new Tuple<string>(faultSource));
}
{code:c#}
