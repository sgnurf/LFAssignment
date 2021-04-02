namespace DataIngestion.TestAssignment.Configuration
{
    internal class ElasticSearchConfiguration
    {
        public string Endpoint { get; set; }
        public int IndexingBatchSize { get; set; }
        public int IndexingTimeoutInMinutes { get; set; }
        public string IndexName { get; set; }
    }
}