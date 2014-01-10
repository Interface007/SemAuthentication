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

What do the project contain?
Sem.Authentication.MvcHelper
This is the "core library". It does contain the base classes for authentication 
check attributes or request filters. The name space "InAppIps" does stand for
"in-application intrusion prevention system" as this part has been implemented 
to enable the developer to build analysis for request pattern right into the
application (e.g. by specifying the expected pause a human would do when 
requesting the same page a 2nd time).
Sem.Authentication.MvcHelper.Yubico
This implements the YUBICO specific parts of a "AuthenticationCheckAttribute".
This way all YUBICO specific code has been removed from the "core", so you can 
implement a "AuthenticationCheckAttribute" without a reference to YUBICO.
Sem.Authentication.MvcHelper.WebSite
This is a sample web application that does show how to apply the different 
protection mechanisms to a web application.
Sem.Authentication.MvcHelper.Test
This provides test for the code inside Sem.Authentication.MvcHelper
Sem.Authentication.MvcHelper.Yubico.Test
This provides test for the code inside Sem.Authentication.MvcHelper.Yubico.Test

Why are the unit test classes wrapped inside a static class?
Each test class bundles the tests for one method. Each of the static classes
bundles the test classes for all methods of one class under test. This way the
test results are organized in a way you can easily group.

Are you cheating with the 100% code coverage?
I'm a big fan of 100% code coverage for unit tests. But I also know there are 
certain situations that do produce code that cannot be covered or does not need
to be covered by unit tests. One example of this is "YubicoClientAdapter". The
purpose of this class is to be able to "hide" the sealed external class 
"YubicoClient" behind an interface. The class does not contain any business 
logic and a test of this class would be an integration test by definition.
Because of this I add the ExcludeFromCodeCoverageAttribute to the class to
hide this class completely from code coverage. This means "I have thought about
the consequences and effects of not writing unit tests for this class and I
came to the conclusion that it's better to not test this class with unit 
tests". Not excluding such code from code coverage would mean that I have n% 
of untested code in the code coverage result and I don't know how much of that
has been reviewed and really should not been tested and how much I still need
to have a look for. Finally it's like the code analysis exclusions: you need
them, because there are exceptions from the generic rules and you want to 
distinguish such exceptions from things you did not correct yet.