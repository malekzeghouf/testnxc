namespace Neoledge.Nxc.Domain.Api.Message
{
    public class ReceivedMessageResponse
    {
        public required string InboxId { get; init; }

        public required Dictionary<string, string> InboxParameters { get; init; }

        public required Guid MessageId { get; init; }

        public long MessageBodySize { get; init; }

        public required MessageSealParameters MessageBodySeal { get; init; }

        public required string MemberId { get; init; }

        public required string MemberName { get; init; }

        public string? MemberDescription { get; init; }

        public DateTime MessageDepositUtcDate { get; init; }

        public DateTime? MessageLastReceivedUtcDate { get; init; }

        public int MessageReceivedCount { get; init; }

        public string? InvisibleUntilUtcDate { get; init; }
    }
}