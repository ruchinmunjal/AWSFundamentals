﻿using Customer.Consumer.Api.Messages;
using MediatR;

namespace Customer.Consumer.Api.Handlers
{
    public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
    {
        private readonly ILogger<CustomerUpdatedHandler> _logger;

        public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
        {
            _logger = logger;
        }

        public Task<Unit> Handle(CustomerUpdated request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message request with GithubName :{gitHubUserName} received", request.GitHubUserName);
            return Unit.Task;
        }
    }
}

