// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGate.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the IGate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication
{
    using System.Collections.Concurrent;
    using System.Web;

    using Sem.Authentication.Processing;

    /// <summary>
    /// The gate interface abstracts the processing of the requests from the APS.Net framework (WebAPI, MVC, ...).
    /// </summary>
    public interface IGate
    {
        /// <summary>
        /// Gets a value indicating whether to check the statistic gate. If this property 
        /// is overridden, it can control whether to check the statistics of a request using the method
        /// <see cref="StatisticsGate"/>. If you override this property you can return "false" in order to 
        /// prevent time consuming processing that only applies to calling <see cref="StatisticsGate"/>.
        /// </summary>
        bool CheckStatisticGate { get; }

        /// <summary>
        /// Gets a value indicating whether to check the request gate. If this property 
        /// is overridden, it can control whether to check the request of a request using the method
        /// <see cref="RequestGate"/>. If you override this property you can return "false" in order to 
        /// prevent time consuming processing that only applies to calling <see cref="RequestGate"/>.
        /// </summary>
        bool CheckRequestGate { get; }

        /// <summary>
        /// The statistics gate does check the clients statistics and prohibits further processing of the request if the client
        /// requests this resource too often. You should set <see cref="CheckStatisticGate"/> to false inside your attribute 
        /// implementation if it does not override this method.
        /// </summary>
        /// <param name="clientId"> The client ID may be a session id or a client IP.  </param>
        /// <param name="statistics"> The statistics collection that match the type of the client id.  </param>
        /// <returns> A value indicating whether the client is allowed to go on. </returns>
        bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics);

        /// <summary>
        /// The request gate does check the content or meta data of the request. You should set <see cref="CheckRequestGate"/>
        /// to false inside your attribute implementation if it does not override this method.
        /// </summary>
        /// <param name="clientId"> The client id. </param>
        /// <param name="request"> The request. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        bool RequestGate(string clientId, HttpRequestBase request);
    }
}
