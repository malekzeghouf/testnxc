using Microsoft.EntityFrameworkCore;
using Neoledge.NxC.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Database
{
    public interface IAppDbContext
    {
        DbSet<Federation> Federations { get; }
        DbSet<Member> Members { get; }
        DbSet<FederationContact> FederationContacts { get; }
        DbSet<InboxPublicCertificate> InboxPublicCertificates { get; }
        DbSet<Inbox> Inboxes { get; }
        DbSet<InboxRestrictedMember> InboxRestrictedMembers { get; }
        DbSet<MessageHeader> MessageHeaders { get; }
        DbSet<MessageState> MessageStates { get; }
        DbSet<MessageHistory> MessageHistories { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
