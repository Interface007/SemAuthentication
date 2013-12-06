// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubikeyCheck.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubikeyCheck type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.Exceptions;

    using YubicoDotNetClient;

    /// <summary>
    /// MVC filter attribute to add a request filter that does check for a YUBIKEY and its validity.
    /// </summary>
    public class YubikeyCheck : AuthenticationCheck
    {
        /// <summary>
        /// The server configuration.
        /// </summary>
        private readonly YubikeyConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyCheck"/> class.
        /// </summary>
        public YubikeyCheck()
            : this(YubikeyConfiguration.DeserializeConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YubikeyCheck"/> class.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        public YubikeyCheck(YubikeyConfiguration configuration)
            : base(configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the name of the image to be returned for this authenticator.
        /// </summary>
        protected override string ImageName
        {
            get
            {
                this.configuration.EnsureCorrectConfiguration();
                return "yubikey-finger.png";
            }
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void InternalAuthenticationCheck(ActionExecutingContext filterContext)
        {
            this.configuration.EnsureCorrectConfiguration();

            // to validate, we need the value of the key - have a look if we can find it.
            var parameters = filterContext.HttpContext.Request.Form;
            var containskey = parameters.Keys.OfType<string>().Contains("yubiKey");
            if (!containskey)
            {
                throw new YubikeyNotPresentException();
            }

            var otp = parameters["yubiKey"];

            var server = this.configuration.Server;
            var client = new YubicoClient(server.ClientId);
            client.SetApiKey(server.ApiKey);
            client.SetSync(server.SyncLevel);

            IYubicoResponse response;
            try
            {
                response = client.Verify(otp);
            }
            catch (Exception ex)
            {
                throw new YubikeyInvalidResponseException(YubicoResponseStatus.BackendError, ex);
            }

            if (response == null)
            {
                throw new YubikeyNullResponseException();
            }

            var user = filterContext.HttpContext.User;
            var users = this.configuration.Users;
            
            // the response must be OK 
            // the name of the current http context identity must match, OR SkipIdentityNameCheck must be enabled
            if (response.Status == YubicoResponseStatus.Ok 
             && ((this.SkipIdentityNameCheck && users.Any(x => x.ExternalId == response.PublicId))
              || (users.FirstOrDefault(x => x.ExternalId == response.PublicId).Name == user.Identity.Name)))
            {
                return;
            }

            throw new YubikeyInvalidResponseException(response.Status);
        }
    }

    public class FastRequestsProtectionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The session statistics.
        /// </summary>
        private readonly ConcurrentDictionary<string, ClientStatistic> sessionStatistics = new ConcurrentDictionary<string, ClientStatistic>();

        /// <summary>
        /// The user host statistics.
        /// </summary>
        private readonly ConcurrentDictionary<string, ClientStatistic> userHostStatistics = new ConcurrentDictionary<string, ClientStatistic>();

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Where we collect come information about the client request and update the statistics. We also will prevent further request processing by throwing exceptions
        /// if the statists do tell us that this client is an attacker.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // todo: null checks
            var session = filterContext.RequestContext.HttpContext.Session;
            if (session != null)
            {
                var sessionId = session.SessionID;
                this.StatisticsGate(sessionId, this.sessionStatistics);
            }

            var userHost = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
            this.StatisticsGate(userHost, this.userHostStatistics);

            base.OnActionExecuting(filterContext);
        }

        private void StatisticsGate(string sessionId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            // todo: make it configurable
            // cleanup old statistics (we will not take care for clients that are slower than 1 request per 30 sec.)
            foreach (var statistic in statistics.Where(x => x.Value.LastRequest < DateTime.UtcNow.AddSeconds(-30)).ToArray())
            {
                ClientStatistic value;
                statistics.TryRemove(statistic.Key, out value);
            }

            var sessionStats = statistics.GetOrAdd(sessionId, new ClientStatistic());
            sessionStats.IncreaseRequestCount();
            if (sessionStats.RequestsPerSecond > this.RequestsPerSecondAndClient)
            {
                throw new HttpException(403, "Request denied for this client.");
            }
        }

        public int RequestsPerSecondAndClient { get; set; }
    }

    internal class ClientStatistic
    {
        private int requestCount;

        public ClientStatistic()
        {
            this.LastRequest = DateTime.UtcNow;
        }

        public void IncreaseRequestCount()
        {
            if (this.requestCount > 1 && this.LastRequest < DateTime.UtcNow.AddSeconds(10))
            {
                this.requestCount = this.requestCount / 2;
            }

            this.LastRequest = DateTime.UtcNow;
            this.requestCount++;
        }

        public DateTime LastRequest { get; set; }

        public int RequestsPerSecond
        {
            get
            {
                return this.requestCount;
            }
        }
    }
}
