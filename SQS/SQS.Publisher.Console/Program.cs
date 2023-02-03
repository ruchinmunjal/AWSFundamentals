
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using SQS_sample;
using System.Text.Json;



var client = new AmazonSQSClient(new AmazonSQSConfig
{
    RegionEndpoint = RegionEndpoint.EUWest2
});

var customer = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "John Doe",
    Email = "emailjohn@whatever.com",
    DateOfBirth = new DateTime(1983, 10, 01),
    GitHubUserName = "ruchinmunjal"
};

var queueUrlResponse = await client.GetQueueUrlAsync("customers");

var sendMessageRequest = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType",new MessageAttributeValue
            {
                DataType="String",
                StringValue=nameof(CustomerCreated)

            }
        }
    }

};

var response = await client.SendMessageAsync(sendMessageRequest);
Console.WriteLine($"Message received with Id: {response.MessageId}");
Console.ReadLine();



