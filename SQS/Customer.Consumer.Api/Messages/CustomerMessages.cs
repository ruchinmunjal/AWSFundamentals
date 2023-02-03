using MediatR;

namespace Customer.Consumer.Api.Messages
{
    public class CustomerCreated : ISQSMessage
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }

        public required string GitHubUserName { get; set; }

        public required DateTime DateOfBirth { get; set; }

    }

    public class CustomerUpdated : ISQSMessage
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }

        public required string GitHubUserName { get; set; }

        public required DateTime DateOfBirth { get; set; }
    }
    public class CustomerDeleted : ISQSMessage
    {
        public required Guid Id { get; set; }
    }


}
