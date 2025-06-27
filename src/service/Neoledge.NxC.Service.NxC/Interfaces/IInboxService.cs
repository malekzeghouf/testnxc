using Neoledge.Nxc.Domain.Api.Inbox;

namespace Neoledge.NxC.Service.NxC.Interfaces
{
    public interface IInboxService
    {
        Task<IList<InboxResponse>> GetMyInboxesAsync(string federationId, string memberId, CancellationToken cancellationToken);

        Task<IList<InboxResponse>> GetInboxesForMemberAsync(string federationId, string memberId, string forMemberId, CancellationToken cancellationToken);

        Task<string> GetInboxPublicCertificateAsync(string federationId, string inboxId, CancellationToken cancellationToken);

        Task<bool> PublishPublicCertificateAsync(string federationId, string memberId, string inboxId, string publicCert, CancellationToken cancellationToken);
    }
}