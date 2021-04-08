namespace Cms.Host
{
    public class HostConfiguration
    {
        public string ClusterId { get; set; }
        public string ServiceId { get; set; }
        public bool CreateShardKey { get; set; } = false;
        public bool UseLocalCluster { get; set; }
        public MongoDbHostConfig MongoDb { get; set; }
    }

    public class MongoDbHostConfig
    {
        public string ClusteringDatabase { get; set; }
        public string Database { get; set; }
        public string ConnectionString { get; set; }
    }
}
