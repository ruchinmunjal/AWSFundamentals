using Customer.Consumer.Api.Messages;
using MediatR;

namespace Customer.Consumer.Api.Handlers
{
    public class CustomerCreatedHandler : IRequestHandler<CustomerCreated>
    {
        private readonly ILogger<CustomerCreatedHandler> _logger;

        public CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CustomerCreated request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Message request with FullName:{fullName} received", request.FullName);

            return Unit.Task;
        }
    }
}
