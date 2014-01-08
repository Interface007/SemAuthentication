// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MvcTestBase.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the MvcTestBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test
{
    using System;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;

    /// <summary>
    /// The test base class for MVC tests. This class does provide some basic helpers for
    /// testing MVC controllers / filters etc.
    /// </summary>
    public class MvcTestBase
    {
        /// <summary>
        /// Creates a new request context that returns a distinct, but always the same, session id.
        /// </summary>
        /// <returns> The <see cref="Mock"/> object for the request context. </returns>
        public static ActionExecutingContext CreateRequestContext()
        {
            return CreateRequestContext(new Uri("http://localhost/test", UriKind.Absolute), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Creates a new request context that returns a distinct, but always the same, session id.
        /// </summary>
        /// <param name="requestUrl">The URL that should be simulated to be requested.</param>
        /// <returns> The <see cref="Mock"/> object for the request context.  </returns>
        public static ActionExecutingContext CreateRequestContext(Uri requestUrl)
        {
            return CreateRequestContext(requestUrl, Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Creates a new request context that returns a distinct, but always the same, session id.
        /// </summary>
        /// <param name="requestUrl">The URL the request should fake.</param>
        /// <param name="sessionId"> The session Id for the request. </param>
        /// <param name="clientIP"> The client IP for the request. </param>
        /// <returns> The <see cref="Mock"/> object for the request context.  </returns>
        public static ActionExecutingContext CreateRequestContext(Uri requestUrl, string sessionId, string clientIP)
        {
            return CreateRequestContext(requestUrl, sessionId, clientIP, new NameValueCollection());
        }

        /// <summary>
        /// Creates a new request context that returns a distinct, but always the same, session id.
        /// </summary>
        /// <param name="requestUrl">The URL the request should fake.</param>
        /// <param name="sessionId"> The session Id for the request. </param>
        /// <param name="clientIP"> The client IP for the request. </param>
        /// <param name="formCollection">The collection of form data items.</param>
        /// <returns> The <see cref="Mock"/> object for the request context.  </returns>
        public static ActionExecutingContext CreateRequestContext(Uri requestUrl, string sessionId, string clientIP, NameValueCollection formCollection)
        {
            return CreateRequestContext(requestUrl, sessionId, clientIP, formCollection, "UserName");
        }

        /// <summary>
        /// Creates a new request context that returns a distinct, but always the same, session id.
        /// </summary>
        /// <param name="requestUrl">The URL the request should fake.</param>
        /// <param name="sessionId"> The session Id for the request. </param>
        /// <param name="clientIP"> The client IP for the request. </param>
        /// <param name="formCollection">The collection of form data items.</param>
        /// <param name="userName">The name of the user the identity should contain.</param>
        /// <returns> The <see cref="Mock"/> object for the request context.  </returns>
        public static ActionExecutingContext CreateRequestContext(Uri requestUrl, string sessionId, string clientIP, NameValueCollection formCollection, string userName)
        {
            object values = new
                                {
                                    controller = "Home",
                                    action = "Index",
                                    id = UrlParameter.Optional
                                };

            var requestContext = RequestContext(requestUrl, sessionId, clientIP, formCollection, values, userName);
            var routes = RouteCollection(values);

            var controller = Controller(requestContext, routes);

            return new ActionExecutingContext
                       {
                           RequestContext = requestContext,
                           Controller = controller.Object,
                       };
        }

        /// <summary>
        /// Creates an initialized <see cref="HtmlHelper"/> object.
        /// </summary>
        /// <returns>
        /// The <see cref="HtmlHelper"/>.
        /// </returns>
        public static HtmlHelper CreateHtmlHelper()
        {
            var container = new Mock<IViewDataContainer>();
            return new HtmlHelper(ViewContext(new Uri("http://test/"), Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"), new NameValueCollection()), container.Object);
        }

        private static RequestContext RequestContext(Uri requestUrl, string sessionId, string clientIP, NameValueCollection formCollection, object values, string username)
        {
            var requestBase = RequestBase(requestUrl, clientIP, formCollection);
            var httpContext = HttpContext(sessionId, requestBase, username);
            return RequestContext(httpContext, values);
        }

        private static Mock<Controller> Controller(RequestContext requestContext, RouteCollection routes)
        {
            var urlHelper = new UrlHelper(requestContext, routes);
            var controller = new Mock<Controller>();
            controller.Object.Url = urlHelper;
            RouteTable.Routes.Clear();
            foreach (var route in routes)
            {
                RouteTable.Routes.Add(route);
            }

            return controller;
        }

        private static RouteCollection RouteCollection(object values)
        {
            return new RouteCollection
                             {
                                 new Route("{controller}/{action}/{id}", new MvcRouteHandler())
                                     {
                                         Defaults = new RouteValueDictionary(values),
                                         Constraints = new RouteValueDictionary((object)null),
                                         DataTokens = new RouteValueDictionary(),
                                     }
                             };
        }

        private static HttpContextBase HttpContext(string sessionId, HttpRequestBase requestBase, string username)
        {
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.Session).Returns(SessionState(sessionId).Object);
            httpContext.Setup(x => x.Request).Returns(requestBase);
            httpContext.Setup(x => x.Response).Returns(Response().Object);
            httpContext.Setup(x => x.User).Returns(User(username));
            return httpContext.Object;
        }

        private static IPrincipal User(string username)
        {
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();
            identity.Setup(x => x.Name).Returns(username);
            user.Setup(x => x.Identity).Returns(identity.Object);
            return user.Object;
        }

        private static Mock<HttpResponseBase> Response()
        {
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);
            return response;
        }

        private static Mock<HttpSessionStateBase> SessionState(string sessionId)
        {
            var sessionState = new Mock<HttpSessionStateBase>();
            sessionState.Setup(x => x.SessionID).Returns(sessionId);
            return sessionState;
        }

        private static HttpRequestBase RequestBase(Uri requestUrl, string clientIP, NameValueCollection formCollection)
        {
            var path = requestUrl == null ? string.Empty : requestUrl.AbsolutePath;
            var requestBase = new Mock<HttpRequestBase>();
            requestBase.Setup(x => x.ApplicationPath).Returns(path);
            requestBase.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns("~" + path);
            requestBase.Setup(x => x.CurrentExecutionFilePath).Returns("/");
            requestBase.Setup(x => x.CurrentExecutionFilePathExtension).Returns(string.Empty);
            requestBase.Setup(x => x.Form).Returns(formCollection);
            requestBase.Setup(x => x.HttpMethod).Returns("GET");
            requestBase.Setup(x => x.Path).Returns("/");
            requestBase.Setup(x => x.RawUrl).Returns(path);
            requestBase.Setup(x => x.RequestType).Returns("GET");
            requestBase.Setup(x => x.Url).Returns(requestUrl);
            requestBase.Setup(x => x.UserHostAddress).Returns(clientIP);
            requestBase.Setup(x => x.UserHostName).Returns(clientIP);
            return requestBase.Object;
        }

        private static RequestContext RequestContext(HttpContextBase httpContext, object values)
        {
            var requestContext = new Moq.Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(httpContext);
            requestContext.Setup(x => x.RouteData).Returns(RouteData(values));

            return requestContext.Object;
        }

        private static RouteData RouteData(object values)
        {
            var routeData = new RouteData
                                {
                                    Route =
                                        new Route("{controller}/{action}/{id}", new MvcRouteHandler())
                                            {
                                                Defaults = new RouteValueDictionary(values),
                                                Constraints = new RouteValueDictionary((object)null),
                                                DataTokens = new RouteValueDictionary()
                                            },
                                };

            routeData.Values.Add("controller", "Home");
            routeData.Values.Add("action", "Index");
            return routeData;
        }

        private static ViewContext ViewContext(Uri requestUrl, string sessionId, string clientIP, NameValueCollection formCollection)
        {
            object values = new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                };

            var viewContext = new Mock<ViewContext>();
            var requestContext = RequestContext(requestUrl, sessionId, clientIP, formCollection, values, "userName");
            var routes = RouteCollection(values);
            var controller = Controller(requestContext, routes);
            viewContext.Setup(x => x.Controller).Returns(controller.Object);
            return viewContext.Object;
        }
    }
}