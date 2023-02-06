
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SNS.Publisher.Console.Contracts;
using System.Text.Json;

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "Bruce Wayne",
    DateOfBirth = new DateTime(1908, 01, 10),
    Email = "TheGreatestDetective@batcave.com",
    GitHubUserName = "darkKnight"

};

var cts = new CancellationTokenSource();
var snsClient = new AmazonSimpleNotificationServiceClient();
var topicArn = (await snsClient.FindTopicAsync("customers")).TopicArn;

var publishRequest = new PublishRequest
{
    TopicArn = topicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType",new MessageAttributeValue
            {
                DataType ="String",
                StringValue ="CustomerCreated"
            }
        }
    }

};

var response = await snsClient.PublishAsync(publishRequest);

