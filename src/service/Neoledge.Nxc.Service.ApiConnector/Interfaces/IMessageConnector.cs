using Neoledge.Nxc.Domain.Api.Client;
using Neoledge.Nxc.Domain.Api.Inbox;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.Nxc.Domain.Api.Message;

namespace Neoledge.Nxc.Service.ApiConnector.Interfaces
{
    public interface IMessageConnector
    {
        Task<IList<MemberResponse>?> GetMembersAsync(CancellationToken cancellationToken);

        Task<IList<InboxResponse>?> GetInboxesAsync(string memberId, CancellationToken cancellationToken);

        Task<IList<InboxResponse>?> GetMyInboxesAsync(CancellationToken cancellationToken);

        Task<Guid> SendBodyAsync(Stream stream, CancellationToken cancellationToken);

        Task<SendMessageResponse> SendMessageAsync(MessageSendParameters parameters, CancellationToken cancellationToken);



        Task<string> GetInboxPublicCertificateAsync(string inboxId, CancellationToken cancellationToken);
        Task<bool> PublishPublicCertificateAsync(PublishPublicCertificateParameters parameters, CancellationToken cancellationToken);




        Task<PeekMessageResponse?> PeekMessageAsync(CancellationToken cancellationToken);

        Task<ReceivedMessageResponse?> ReceiveMessageAsync(Guid messageId, CancellationToken cancellationToken);

        Task<Stream> ReceiveMessageBodyAsync(Guid messageId, CancellationToken cancellationToken);

        Task<ReceivedMessageResponse?> AckMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken);
    }
}