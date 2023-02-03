using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging
{
    public class SQSMessenger : ISQSMessanger
    {
        private readonly IAmazonSQS _sqs;
        private readonly IOptions<QueueSettings> _queueSettings;
        private string? _queueUrl;

        public SQSMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
        {
            _sqs = sqs;
            _queueSettings = queueSettings;
        }
        public async Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken)
        {
            var queueUrl = await GetQueueUrlAsync();
            var messageRequest = new SendMessageRequest()
            {
                QueueUrl = queueUrl,
                MessageBody = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "MessageType",new MessageAttributeValue
                        {
                            DataType="String",
                            StringValue = typeof(T).Name

                        }
                    }
                }

            };

            return await _sqs.SendMessageAsync(messageRequest, cancellationToken);

        }

        private async Task<string> GetQueueUrlAsync()
        {
            if (_queueUrl is not null)
            {
                return _queueUrl;
            }
            
            _queueUrl= (await _sqs.GetQueueUrlAsync(_queueSettings.Value.QueueName)).QueueUrl;
            return _queueUrl;
        }
    }
}
