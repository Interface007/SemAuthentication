namespace Sem.Authentication.MvcHelper.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;

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
        /// <param name="requestUrl"></param>
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
            // we setup a request context that returns always the same session id
            var sessionState = new Mock<HttpSessionStateBase>();
            sessionState.Setup(x => x.SessionID).Returns(sessionId);

            var requestBase = new Mock<HttpRequestBase>();
            requestBase.Setup(x => x.UserHostAddress).Returns(clientIP);
            requestBase.Setup(x => x.ApplicationPath).Returns("/");
            requestBase.Setup(x => x.Url).Returns(requestUrl);

            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);

            var httpContext = new Moq.Mock<HttpContextBase>();
            httpContext.Setup(x => x.Session).Returns(sessionState.Object);
            httpContext.Setup(x => x.Request).Returns(requestBase.Object);
            httpContext.Setup(x => x.Response).Returns(response.Object);

            var requestContext = new Moq.Mock<RequestContext>();
            requestContext.Setup(x => x.HttpContext).Returns(httpContext.Object);
            requestContext.Setup(x => x.RouteData).Returns(new RouteData());

            var controller = new Mock<Controller>();
            var routeCollection = new RouteCollection
                                      {
                                          new Route("{controller}/{action}/{id}", new MvcRouteHandler())
                                              {
                                                  Defaults = CreateRouteValueDictionary(
                                                  new
                                                      {
                                                          controller = "Home", 
                                                          action = "Index", 
                                                          id = UrlParameter.Optional
                                                      }), 
                                                  Constraints = CreateRouteValueDictionary(null), 
                                                  DataTokens = new RouteValueDictionary()
                                              }
                                      };

            controller.Object.Url = new UrlHelper(requestContext.Object, routeCollection);

            return new ActionExecutingContext
                       {
                           RequestContext = requestContext.Object,
                           Controller = controller.Object,
                       };
        }

        /// <summary>
        /// Creates a route value dictionary.
        /// </summary>
        /// <param name="values"> The values. </param>
        /// <returns> The <see cref="RouteValueDictionary"/>. </returns>
        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            return new RouteValueDictionary(values);
        }
    }
}