

using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;

var cts = new CancellationTokenSource();


var sqsClient = new AmazonSQSClient(RegionEndpoint.EUWest2);
var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");
var queueUrl = queueUrlResponse.QueueUrl;

var receivedMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrl,
    AttributeNames = new List<string> { "All" },
    MessageAttributeNames = new List<string> { "MessageType" }
};

while (!cts.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(receivedMessageRequest,cts.Token);
    if (response != null)
    {
        foreach (var message in response.Messages)
        {
            Console.WriteLine($"Message Id id {message.MessageId}");

            Console.WriteLine($"Message Body is : {message.Body}");

            Console.WriteLine($"Message Attributes are : {JsonSerializer.Serialize(message.MessageAttributes["MessageType"].StringValue)}");

            //If everything worked, delete the message from queue
            await sqsClient.DeleteMessageAsync(queueUrl, message.ReceiptHandle);
        }
    }

    await Task.Delay(1000);
}

