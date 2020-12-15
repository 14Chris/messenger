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

                    switch (cr.Topic)
                    {
                        case "conversation_created":
                            int id = -1;

                            //Convert the message value containing the created conversation id
                            bool valueOk = Int32.TryParse(cr.Message.Value, out id);

                            //If the value is converted succefully
                            if (valueOk)
                            {
                                //Created a service factory to get a instance of the service
                                using (var scope = _serviceScopeFactory.CreateScope())
                                {
                                    //Send the conversation created to all users
                                    scope.ServiceProvider.GetRequiredService<ICommunicationService>().SendConversationCreatedNotification(id);
                                }

                            }
                            break;

                        case "message_created":
                            break;
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
