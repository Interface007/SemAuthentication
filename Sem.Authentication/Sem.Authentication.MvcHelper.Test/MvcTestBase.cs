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
            object values = new
                                {
                                    controller = "Home",
                                    action = "Index",
                                    id = UrlParameter.Optional
                                };

            var requestBase = RequestBase(requestUrl, clientIP, formCollection);
            var httpContext = HttpContext(sessionId, requestBase);
            var requestContext = RequestContext(httpContext, values);
            var routes = RouteCollection(values);

            var urlHelper = new UrlHelper(requestContext.Object, routes);
            var controller = new Mock<Controller>();
            controller.Object.Url = urlHelper;
            RouteTable.Routes.Clear();
            foreach (var route in routes)
            {
                RouteTable.Routes.Add(route);                
            }

            return new ActionExecutingContext
                       {
                           RequestContext = requestContext.Object,
                           Controller = controller.Object,
                       };
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

        private static Mock<HttpContextBase> HttpContext(string sessionId, Mock<HttpRequestBase> requestBase)
        {
            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.Session).Returns(SessionState(sessionId).Object);
            httpContext.Setup(x => x.Request).Returns(requestBase.Object);
            httpContext.Setup(x => x.Response).Returns(Response().Object);
            return httpContext;
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

        private static Mock<HttpRequestBase> RequestBase(Uri requestUrl, string clientIP, NameValueCollection formCollection)
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
            return requestBase;
        }

        private static Mock<RequestContext> RequestContext(Mock<HttpContextBase> httpContext, object values)
        {
            var requestContext = new Moq.Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(httpContext.Object);
            requestContext.Setup(x => x.RouteData).Returns(RouteData(values));

            return requestContext;
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
    }
}