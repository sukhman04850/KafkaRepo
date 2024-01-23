namespace ProductService.KafkaConfiguration
{
    public class KafkaConfig
    {
        public required string BootstrapServers { get; set; }
        public required string TopicName { get; set; }
        public required string SaslUsername { get; set; }
        public required string SaslPassword { get; set; }
    }
}
