using Neoledge.Nxc.Domain.Api.Message;

namespace Neoledge.NxC.Service.NxC.Interfaces
{
    public interface IMessageService
    {
        Task<Guid> SendBodyAsync(Stream file, string contentType, CancellationToken cancellationToken);

        Task<Guid> SendAsync(MessageSendParameters parameters, string senderInboxId, string senderMemberId, string federationId, CancellationToken cancellationToken);

        Task<Guid?> PeekMessageAsync(string memberId, string federationId, CancellationToken cancellationToken);

        Task<bool> GetMessageBodyAsync(Guid messageId, Stream file, CancellationToken cancellationToken);

        Task<ReceivedMessageResponse> ReceivedMessageAsync(Guid messageId, CancellationToken cancellationToken);

        //without logs or message to return
        Task<ReceivedMessageResponse> AckMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken);

        Task<ReceivedMessageResponse> NaKMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken);

        Task<ReceivedMessageResponse> RejectMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken);
    }
}