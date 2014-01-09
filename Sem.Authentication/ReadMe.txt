BEFORE YOU PRESS F5:
The web project Sem.Authentication.MvcHelper.WebSite does provode some sample
pages using the features of this library. In order to run this sample web site
you MUST copy the file "YubiKey.xml" from the project 
Sem.Authentication.MvcHelper.Yubico to the root folder of the web application.
This file does contain the configuration for the YUBICO authentication client.
This information does contain an ID and a key you need to request from YUBICO
if you want to use the 2nd factor authentication - the values provided in the 
sample config xml from Sem.Authentication.MvcHelper.Yubico are synthactical 
ok, but just dummy values that will be rejected when trying to access the
official YUBICO servers.

--------------------------------------------------------------------------------

This library provides some "tools" for dealing with authentication like MVC 
attributes to add another authentication factor when accessing specific MVC 
actions.

The purpose of this project is to provide tools for an MVC application developer 
to ease some tasks in the field of authentication and authorization.

What problems does it solve?
In web applications you face issues like getting a 2nd factor of authentication 
for specific actions or preventing the usage of attack tools like havij 
(http://itsecteam.com/products/havij-advanced-sql-injection/). This library 
provides you with easy to use ASP.Net MVC attributes to handle such challenges.

Current feature list:
    Attribute based additional authentication factor for MVC controller actions
		The attribute "YubikeyCheck" validates a YubiKey(http://www.yubico.com/)
		token string and redirects to a specific page if the token is not valid.
		If you have a different 2nd factor, you can inherit from an abstract 
		base class “AuthenticationCheck” that does provide some of the 
		"infrastructure" for such a check.
    Attribute based throttling of requests from one client
		The attribute "MinimumRequestTimeDistance" allows to set a minimum of 
		time between two requests from a single client for a specific controller
		action. With the attribute "FastRequestsProtection" you can specify how 
		many requests per second can be issued from one client (this allows the 
		client to issue "n" requests simultaneously, but then the client has to 
		wait). Both attributes will lower the effectiveness of automated attack 
		tools that mostly rely on a mass of requests issued in short time.