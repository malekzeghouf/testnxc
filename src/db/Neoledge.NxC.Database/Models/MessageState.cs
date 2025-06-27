using Neoledge.Nxc.Domain.Database.Enum;

namespace Neoledge.NxC.Database.Models
{
    public class MessageState
    {
        public int Id { get; init; }

        public Guid MessageId { get; init; }

        public required MessageStatus Status { get; set; }

        public DateTime? Expiration { get; init; }

        public DateTime? LeasedUtcUntil { get; set; }

        public DateTime? LeasedUtcAt { get; set; }

        public MessageHeader? MessageHeader { get; init; }
    }
}