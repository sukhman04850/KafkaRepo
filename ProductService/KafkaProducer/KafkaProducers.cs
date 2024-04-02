using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductService.KafkaConfiguration;
namespace ProductService.KafkaProducer
{
    public class KafkaProducers
    {
        private readonly IProducer<string, string> producer;
        private readonly string topic;

        public KafkaProducers(IOptions<KafkaConfig> kafkaConfig)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaConfig.Value.BootstrapServers,
                
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = kafkaConfig.Value.SaslUsername,
                SaslPassword = kafkaConfig.Value.SaslPassword
            };

            producer = new ProducerBuilder<string, string>(config).Build();
            topic = kafkaConfig.Value.TopicName ?? throw new Exception("Error: TOpic Is Null");
           
        }

        public async Task Message(string key, int productId, float productPrice, int quantity)
        {
            var realMessage = new { ProductID = productId, Price = productPrice, Quantity = quantity };
            var newKey = JsonConvert.SerializeObject(key);
            var newMessage = JsonConvert.SerializeObject(realMessage);

            await producer.ProduceAsync(topic, new Message<string, string> { Key = newKey, Value = newMessage });
            Console.WriteLine("This is the message for Kafka:" + key + realMessage);
        }

    }
}
