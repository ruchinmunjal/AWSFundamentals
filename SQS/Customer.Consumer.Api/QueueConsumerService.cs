using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customer.Consumer.Api.Messages;
using MediatR;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Customer.Consumer.Api
{
    public class QueueConsumerService : BackgroundService
    {

        private readonly IAmazonSQS _sqs;
        private readonly IOptions<QueueSettings> _settings;
        private readonly IMediator _mediator;
        private readonly ILogger<QueueConsumerService> _logger;

        public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> settings, IMediator mediator, ILogger<QueueConsumerService> logger)
        {
            _sqs = sqs;
            _settings = settings;
            _mediator = mediator;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {


            var sqsClient = new AmazonSQSClient(RegionEndpoint.EUWest2);
            var queueUrlResponse = await _sqs.GetQueueUrlAsync(_settings.Value.QueueName, stoppingToken);
            var queueUrl = queueUrlResponse.QueueUrl;


            var receivedMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                AttributeNames = new List<string> { "All" },
                MessageAttributeNames = new List<string> { "MessageType" },
                MaxNumberOfMessages = 1
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await sqsClient.ReceiveMessageAsync(receivedMessageRequest, stoppingToken);
                if (response != null)
                {
                    foreach (var message in response.Messages)
                    {
                        var messageType = message.MessageAttributes["MessageType"].StringValue;
                        var type = Type.GetType($"Customer.Consumer.Api.Messages.{messageType}");

                        if (type is null)
                        {
                            _logger.LogWarning("Unknown message type : {MessageType}", messageType);
                            continue;
                        }
                        var typedMessage = (ISQSMessage)JsonSerializer.Deserialize(message.Body, type)!;

                        try
                        {
                            await _mediator.Send(typedMessage, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Message failed during processing");
                            continue;
                        }

                        //switch (messageType)
                        //{
                        //    case nameof(CustomerCreated):
                        //        var created = JsonSerializer.Deserialize<CustomerCreated>(message.Body);
                        //        break;
                        //    case nameof(CustomerUpdated):
                        //        break;
                        //    case nameof(CustomerDeleted):
                        //        break;
                        //}


                        //If everything worked, delete the message from queue
                        await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);

                    }
                }

                await Task.Delay(1000);
            }

            return;

        }
    }
}
