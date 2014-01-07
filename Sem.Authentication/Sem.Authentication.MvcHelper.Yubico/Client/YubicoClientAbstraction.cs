// --------------------------------------------------------------------------------------------------------------------
// <copyright file="YubicoClientAbstraction.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the YubicoClientAbstraction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Client
{
    using System.Diagnostics.CodeAnalysis;

    using YubicoDotNetClient;

    public class YubicoClientAbstraction : IYubicoClient
    {
        public string ClientId { get; set; }

        public string ApiKey { get; set; }

        public string SyncLevel { get; set; }

        [ExcludeFromCodeCoverage]
        public IYubicoResponse Verify(string otp)
        {
            var client = new YubicoClient(this.ClientId);
            client.SetApiKey(this.ApiKey);
            client.SetSync(this.SyncLevel);
            return client.Verify(otp);
        }
    }
}