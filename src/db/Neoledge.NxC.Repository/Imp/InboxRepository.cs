using Microsoft.EntityFrameworkCore;
using Neoledge.Nxc.Domain.Exceptions;
using Neoledge.NxC.Database;
using Neoledge.NxC.Database.Models;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Repository.Imp
{
    internal class InboxRepository(IAppDbContext context) : IInboxRepository
    {
        //TODO: Implement the methods for managing Inboxes
        public Task<Inbox> AddInboxAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Inbox>> GetInboxesAsync(string memberId, CancellationToken cancellationToken)
        {
            return await context.Inboxes.Where(i => i.MemberId == memberId).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<Inbox>> GetInboxesForMemberAsync(string memberId, string forMemberId, CancellationToken cancellationToken)
        {
            return await context.Inboxes.Where(i => i.MemberId == memberId)
                                        .Where(i => !i.InboxRestrictedMembers.Any() || i.InboxRestrictedMembers.Any(r => r.RestrictedMemberId == forMemberId))
                                        .ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Inbox> GetInboxeByIdAsync(string id, CancellationToken cancellationToken)
        {
            var inbox = await context.Inboxes.Include(i => i.InboxParameters).FirstOrDefaultAsync(i => i.Id == id, cancellationToken).ConfigureAwait(false);
            if (inbox == null) throw new EntityNotFoundException(nameof(Inbox), id);
            return inbox;
        }

        // TODO: Implement the methods for managing Inboxes
        public Task<Inbox> RemoveInboxAsync(string memberId, string inboxId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // TODO: Implement the methods for managing Inboxes
        public Task<Inbox> UpdateInboxAsync(string memberId, string inboxId, string newName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<InboxPublicCertificate>> GetInboxCertificatesAsync(string inboxId, CancellationToken cancellationToken)
        {
            var certs = await context.InboxPublicCertificates
                .AsNoTracking()
                .Where(m => m.InboxId == inboxId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            return certs;
        }

        public async Task<string> GetInboxPublicCertificateAsync(string inboxId, CancellationToken cancellationToken)
        {
            var cert = await context.InboxPublicCertificates
                .AsNoTracking()
                .Where(m => m.InboxId == inboxId && m.Active)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (cert == null) throw new EntityNotFoundException(nameof(InboxPublicCertificate), inboxId);
            return cert.PublicCertificatePem;
        }

        public async Task<bool> AddInboxPublicCertificateAsync(string inboxId, string publicCertificate, CancellationToken cancellationToken)
        {
            var cert = await context.InboxPublicCertificates
                .Where(m => m.InboxId == inboxId && m.Active)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (cert != null)
            {
                cert.Active = false;
                context.InboxPublicCertificates.Update(cert);
            }
            await context.InboxPublicCertificates.AddAsync(new InboxPublicCertificate
            {
                InboxId = inboxId,
                PublicCertificatePem = publicCertificate,
                Active = true,
            }, cancellationToken).ConfigureAwait(false);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }
    }
}