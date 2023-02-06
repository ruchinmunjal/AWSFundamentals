using Microsoft.Extensions.Options;
using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Customers.Api.Messaging
{
    public class SNSMessenger : ISnsMessenger
    {
        private readonly IAmazonSimpleNotificationService _sns;
        private readonly IOptions<SnsSettings> _queueSettings;
        private string? _topicArn;

        public SNSMessenger(IAmazonSimpleNotificationService sns, IOptions<SnsSettings> queueSettings)
        {
            _sns = sns;
            _queueSettings = queueSettings;
        }
        private async ValueTask<string> GetTopicArn()
        {
            if (_topicArn is not null)
            {
                return _topicArn;
            }
            
            _topicArn= (await _sns.FindTopicAsync(_queueSettings.Value.TopicName)).TopicArn;
            return _topicArn;
        }

        public async Task<PublishResponse> PublishMessageAsync<T>(T message, CancellationToken cancellationToken)
        {
            var queueUrl = await GetTopicArn();
            var messageRequest = new PublishRequest()
            {
                TopicArn = queueUrl,
                Message = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "MessageType", new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name

                        }
                    }
                }

            };

            return await _sns.PublishAsync(messageRequest, cancellationToken);
        }
    }
}
