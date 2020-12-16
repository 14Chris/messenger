using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Facade.KafkaConfiguration
{
    public class ConsumerWrapper
    {
        private IConsumer<string, string> _consumer;
        private ConsumerConfig _config;
        private string _topicName;
        private static readonly Random rand = new Random();

        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._config = config;
            this._consumer = new ConsumerBuilder<string,string>(this._config).Build();
        }

        public string ReadMessage()
        {
            var consumeResult = this._consumer.Consume();
            return consumeResult.Message.Value;
        }
    }
}
