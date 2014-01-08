// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IYubicoClient.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the IYubicoClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.Yubico.Client
{
    using YubicoDotNetClient;

    public interface IYubicoClient
    {
        string ClientId { get; set; }

        string ApiKey { get; set; }
        
        string SyncLevel { get; set; }

        IYubicoResponse Verify(string onetimePassword);
    }
}
