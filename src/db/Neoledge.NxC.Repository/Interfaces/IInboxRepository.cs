using Neoledge.NxC.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Repository.Interfaces
{
    public interface IInboxRepository
    {
        Task<IList<Inbox>> GetInboxesAsync(string memberId, CancellationToken cancellationToken);

        Task<IList<Inbox>> GetInboxesForMemberAsync(string memberId, string forMemberId, CancellationToken cancellationToken);
        
        Task<Inbox> GetInboxeByIdAsync(string id, CancellationToken cancellationToken);

        Task<Inbox> AddInboxAsync(CancellationToken cancellationToken);

        Task<Inbox> RemoveInboxAsync(string memberId, string inboxId, CancellationToken cancellationToken);

        Task<Inbox> UpdateInboxAsync(string memberId, string inboxId, string newName, CancellationToken cancellationToken);

        Task<IList<InboxPublicCertificate>> GetInboxCertificatesAsync(string inboxId, CancellationToken cancellationToken);

        Task<string> GetInboxPublicCertificateAsync(string inboxId, CancellationToken cancellationToken);

        Task<bool> AddInboxPublicCertificateAsync(string inboxId, string publicCertificate, CancellationToken cancellationToken);
    }
}