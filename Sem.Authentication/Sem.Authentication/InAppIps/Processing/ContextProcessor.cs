// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextProcessor.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   The context processor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.InAppIps.Processing
{
    using System;
    using System.Collections.Concurrent;

    using Sem.Authentication.Processing;

    /// <summary>
    /// The context processor.
    /// </summary>
    public class ContextProcessor
    {
        /// <summary>
        /// The global client statistics - shared by all instances and threads.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, ClientStatistic>> ClientStatistics = new ConcurrentDictionary<Type, ConcurrentDictionary<string, ClientStatistic>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextProcessor"/> class.
        /// </summary>
        /// <param name="idExtractor"> The id extractor to be used with this processor. </param>
        public ContextProcessor(IIdExtractor idExtractor)
        {
            this.IdExtractor = idExtractor;
            this.Statistics = ClientStatistics.GetOrAdd(idExtractor.GetType(), type => new ConcurrentDictionary<string, ClientStatistic>());
        }

        /// <summary>
        /// Gets the id extractor that will extract the unique client id.
        /// </summary>
        public IIdExtractor IdExtractor { get; private set; }

        /// <summary>
        /// Gets the clients statistics for this type of extractor.
        /// </summary>
        public ConcurrentDictionary<string, ClientStatistic> Statistics { get; private set; }
    }
}