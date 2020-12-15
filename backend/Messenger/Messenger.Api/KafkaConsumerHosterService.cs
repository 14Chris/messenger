using Confluent.Kafka;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Api
{

    public class KafkaConsumerHostedService : BackgroundService
    {
        public IServiceScopeFactory _serviceScopeFactory;
        private readonly IConsumer<string, string> kafkaConsumer;

        public KafkaConsumerHostedService(IServiceScopeFactory serviceScopeFactory, ConsumerConfig consumerConfig)
        {
            this._serviceScopeFactory = serviceScopeFactory;

            this.kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();

            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            kafkaConsumer.Subscribe("conversation_created");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this.kafkaConsumer.Consume(cancellationToken);

                    // Handle message...
                    //Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}ms");

                    int id = -1;
                    bool valueOk = Int32.TryParse(cr.Message.Value, out id);

                    if (valueOk)
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            
                            scope.ServiceProvider.GetRequiredService<ICommunicationService>().SendConversationCreatedNotification(id);

                            //Do your stuff
                        }
                        
                    }

                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    // Consumer errors should generally be ignored (or logged) unless fatal.
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }

        public override void Dispose()
        {
            this.kafkaConsumer.Close(); // Commit offsets and leave the group cleanly.
            this.kafkaConsumer.Dispose();

            base.Dispose();
        }

    }
}
