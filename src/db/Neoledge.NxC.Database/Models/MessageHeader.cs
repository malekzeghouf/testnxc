namespace Neoledge.NxC.Database.Models
{
    public class MessageHeader : BaseEntity
    {
        public Guid Id { get; init; }

        public required string FederationId { get; init; }

        public required string SenderMemberId { get; init; }

        public required string RecipientMemberId { get; init; }

        public required string RecipientInboxId { get; init; }

        public required string SendUserCn { get; init; }

        public required string SendUserId { get; init; }

        public long BodySize { get; init; }

        public required byte[] SenderCertificateThumbprint { get; init; }

        public required byte[] RecipientCertificateThumbprint { get; init; }

        public required byte[] SignedHash { get; init; }

        public required byte[] EncryptedKey { get; init; }

        public required byte[] EncryptedIV { get; init; }

        public MessageState MessageState { get; set; } = default!;

        public IList<MessageHistory>? MessageHistories { get; init; }
    };
}