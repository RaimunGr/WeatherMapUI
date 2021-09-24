using Infra.ApplicationServices.Utility.MessageQueuing.Implementations;
using MessageQueuing.Abstractions;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageQueuing.Implementations
{
    public sealed class MessagePuller : BaseMessageHandler, IMessagePuller
    {
        public MessagePuller(string amqpUri)
            : base(amqpUri)
        { }

        public void Pull(string queueName, Action<IDictionary<string, object>, string> onRecieved)
        {
            ValidatePullInputs(queueName, onRecieved);

            CreateChannel();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var headers = ea.BasicProperties.Headers;
                var stringData = Encoding.UTF8.GetString(body);

                onRecieved.Invoke(headers, stringData);

                CreateChannel();
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queueName, false, "", false, false, null, consumer);
        }

        private static void ValidatePullInputs(
            string queueName,
            Action<IDictionary<string, object>, string> onRecieved
        )
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new Exception("Queue name can NOT be null or whitespaces.");
            }

            if (onRecieved is null)
            {
                throw new Exception("OnRecieved can NOT be null.");
            }
        }
    }
}
