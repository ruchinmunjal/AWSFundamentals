using Amazon.SQS.Model;

namespace Customers.Api.Messaging
{
    public interface ISQSMessanger
    {
        Task<SendMessageResponse> SendMessageAsync<T>(T message, CancellationToken cancellationToken);
    }
}
