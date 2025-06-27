using Neoledge.Nxc.Domain.Database.Enum;

namespace Neoledge.NxC.Database.Models
{
    public class MessageHistory
    {
        public int Id { get; init; }

        public Guid MessageId { get; init; }

        public DateTime EventUtcDate { get; init; }

        public required MessageEventCode EventCode { get; init; }

        public int StatusCode { get; init; }

        public required string StatusMessage { get; init; }

        public required string LocalIdentifier { get; init; }

        public MessageHeader? MessageHeader { get; init; }
    }
}