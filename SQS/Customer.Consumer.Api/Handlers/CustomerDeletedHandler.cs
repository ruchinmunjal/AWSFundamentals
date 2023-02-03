using Customer.Consumer.Api.Messages;
using MediatR;

namespace Customer.Consumer.Api.Handlers
{
    public class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
    {
        private readonly ILogger<CustomerDeletedHandler> _logger;

        public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CustomerDeleted request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Customer delete message request with Id :{id} received", request.Id);
            return Unit.Task;
        }
    }
}
