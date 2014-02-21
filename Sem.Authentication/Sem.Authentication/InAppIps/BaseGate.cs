// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseGate.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the BaseGateMvc type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps
{
    using System.Collections.Concurrent;
    using System.Web;

    using Sem.Authentication.Processing;

    /// <summary>
    /// The base gate implementation with defaults.
    /// </summary>
    public abstract class BaseGate : IGate
    {
        /// <summary>
        /// Gets or sets the action to redirect to in case of a fault. 
        /// The action does contain a string parameter <c>FaultSource</c> with the name of this class.
        /// </summary>
        public string FaultAction { get; set; }

        /// <summary>
        /// Gets a value indicating whether to check the statistic gate. If this property 
        /// is overridden, it can control whether to check the statistics of a request using the method
        /// <see cref="StatisticsGate"/>. If you override this property you can return "false" in order to 
        /// prevent time consuming processing that only applies to calling <see cref="StatisticsGate"/>.
        /// </summary>
        public virtual bool CheckStatisticGate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to check the request gate. If this property 
        /// is overridden, it can control whether to check the request of a request using the method
        /// <see cref="RequestGate"/>. If you override this property you can return "false" in order to 
        /// prevent time consuming processing that only applies to calling <see cref="RequestGate"/>.
        /// </summary>
        public virtual bool CheckRequestGate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The request gate does check the content or meta data of the request. You should set <see cref="CheckRequestGate"/>
        /// to false inside your attribute implementation if it does not override this method.
        /// </summary>
        /// <param name="clientId"> The client id. </param>
        /// <param name="request"> The request. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public virtual bool RequestGate(string clientId, HttpRequestBase request)
        {
            return true;
        }

        /// <summary>
        /// The statistics gate does check the clients statistics and prohibits further processing of the request if the client
        /// requests this resource too often. You should set <see cref="CheckStatisticGate"/> to false inside your attribute 
        /// implementation if it does not override this method.
        /// </summary>
        /// <param name="clientId"> The client ID may be a session id or a client IP.  </param>
        /// <param name="statistics"> The statistics collection that match the type of the client id.  </param>
        /// <returns> A value indicating whether the client is allowed to go on. </returns>
        public virtual bool StatisticsGate(string clientId, ConcurrentDictionary<string, ClientStatistic> statistics)
        {
            return true;
        }
    }
}