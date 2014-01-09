// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleAuthenticator.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the SampleAuthenticator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Test.AppInfrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    using Sem.Authentication.MvcHelper.AppInfrastructure;

    [ExcludeFromCodeCoverage]
    public class SampleAuthenticator : AuthenticationCheckAttribute
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
            if (filterContext != null 
             && filterContext.HttpContext != null
             && filterContext.HttpContext.Request != null
             && filterContext.HttpContext.Request.Url != null
             && filterContext.HttpContext.Request.Url.AbsoluteUri.Contains("exception"))
            {
                throw new InvalidOperationException("TESTEXCEPTION");
            }
            
            this.Checked = true;
        }

        public ISemAuthLogger InternalLogger
        {
            get
            {
                return this.Logger;
            }
        }

        public ISemAudit InternalAudit
        {
            get
            {
                return this.Audit;
            }
        }

        public void LogException(Exception exception)
        {
            this.Log(exception);
        }

        public void AuditEvent(Exception exception, ActionExecutingContext filterContext)
        {
            this.AuditFailure(filterContext, exception);
        }

        public void AuditEvent(ActionExecutingContext filterContext)
        {
            this.AuditSuccess(filterContext);
        }
    }
}