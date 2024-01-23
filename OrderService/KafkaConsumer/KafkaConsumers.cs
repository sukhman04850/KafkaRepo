using Confluent.Kafka;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.Interfaces;
using OrderService.KafkaConfiguration;
using OrderService.Model;
using System;
using System.Threading;
using static Confluent.Kafka.ConfigPropertyNames;

namespace OrderService.KafkaConsumer
{
    public class KafkaConsumers /*: IDisposable*/
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceScopeFactory _serviceScope;
        private readonly string _topic;

        public KafkaConsumers(/*IConfiguration configuration*/ IOptions<KafkaConfig> kafkaConfig, IServiceScopeFactory serviceScope)
        {
            var config = new ConsumerConfig
            {
                GroupId = "order-consumer-94",
                BootstrapServers = kafkaConfig.Value.BootstrapServers,
                SaslUsername = kafkaConfig.Value.SaslUsername,
                SaslPassword = kafkaConfig.Value.SaslPassword,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _topic = kafkaConfig.Value.TopicName ?? throw new Exception("Error: Kafka is null");
            _serviceScope = serviceScope;
            _consumer.Subscribe(_topic);
        }
        public async Task ExecuteAsync(CancellationToken tok)
        {

            try
            {
                while (!tok.IsCancellationRequested)
                {

                    await Task.Yield();
                    using (var scope = _serviceScope.CreateScope())
                    {
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderInterface>();

                        try
                        {

                            var consumeResult = await Task.Run(() => _consumer.Consume(tok), tok);

                            if (consumeResult != null && consumeResult.Message != null)
                            {
                                Console.WriteLine(consumeResult.Message);
                                var consumerMessage = JsonConvert.DeserializeObject<Orders>(consumeResult.Message.Value);
                                var key = consumeResult.Message.Key;

                                Console.WriteLine($"Message as Recieved by Consumer: {consumerMessage}");
                                if (consumerMessage == null)
                                {

                                }
                                else
                                {
                                    var id = consumerMessage.ProductId;
                                    var price = consumerMessage.Price;
                                    Console.WriteLine(price);
                                    var quant = consumerMessage.Quantity;
                                    Console.WriteLine(quant);


                                    var table = new Orders()
                                    {
                                        ProductId = id,
                                        Price = price * quant,
                                        Quantity = quant,
                                    };


                                    await orderService.AddOrder(table);
                                    _consumer.Commit();
                                }


                                /*var order = consumerMessage;

                                if (order == null)
                                {
                                    Console.WriteLine("order object is null can't proceed forward");
                                }
                                else
                                {
                                    await orderService.AddOrder(order);
                                }

                                consumer.Commit();*/
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error : {e.Error.Reason}");
                        }
                    }
                }
            }
            finally
            {
               /* consumer.Dispose();*/
                _consumer.Close();
            }
        }
        /*  public void Dispose()
        {
            consumer.Close();
            consumer.Dispose();

        }
*/


    }
}
