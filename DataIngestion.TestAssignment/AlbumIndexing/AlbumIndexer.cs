using DataIngestion.TestAssignment.Configuration;
using DataIngestion.TestAssignment.TargetModels;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DataIngestion.TestAssignment.AlbumIndexing
{
    internal class AlbumIndexer : IAlbumIndexer
    {
        private readonly IElasticClient elasticClient;
        private readonly ElasticSearchConfiguration elasticSearchConfiguration;

        public AlbumIndexer(IElasticClient elasticClient, IOptions<ElasticSearchConfiguration> elasticSearchConfiguration)
        {
            this.elasticClient = elasticClient;
            this.elasticSearchConfiguration = elasticSearchConfiguration.Value;
        }

        public void Index(IEnumerable<Album> albums, CancellationToken cancellationToken)
        {
            BulkAllObservable<Album> bulkAllObservable = elasticClient.BulkAll(albums,
                b => b.Size(elasticSearchConfiguration.IndexingBatchSize), cancellationToken);

            bulkAllObservable.Wait(TimeSpan.FromMinutes(elasticSearchConfiguration.IndexingTimeoutInMinutes), next => { });
        }
    }
}