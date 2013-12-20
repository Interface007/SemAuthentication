namespace Sem.Authentication.MvcHelper.Test.AppInfrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    [ExcludeFromCodeCoverage]
    public class SampleAuthenticator : AuthenticationCheck
    {
        public SampleAuthenticator(ConfigurationBase configuration)
            : base(configuration)
        {
        }

        public bool Checked { get; set; }

        protected override string ImageName
        {
            get
            {
                return "Sample.JPG";
            }
        }

        protected override void InternalAuthenticationCheck(ActionExecutingContext filterContext)
        {
            this.Checked = true;
        }

        public ISemAuthLogger InternalLogger
        {
            get
            {
                return this.Logger;
            }
        }

        public void LogException(Exception exception)
        {
            this.Log(exception);
        }
    }
}