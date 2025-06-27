using Microsoft.EntityFrameworkCore;
using Neoledge.NxC.Database.Models;

namespace Neoledge.NxC.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<Federation> Federations => Set<Federation>();
        public DbSet<FederationContact> FederationContacts => Set<FederationContact>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<InboxPublicCertificate> InboxPublicCertificates => Set<InboxPublicCertificate>();
        public DbSet<MemberContact> MemberContacts => Set<MemberContact>();
        public DbSet<Inbox> Inboxes => Set<Inbox>();
        public DbSet<InboxParameter> InboxParameters => Set<InboxParameter>();
        public DbSet<InboxRestrictedMember> InboxRestrictedMembers => Set<InboxRestrictedMember>();

        public DbSet<MessageHeader> MessageHeaders => Set<MessageHeader>();
        public DbSet<MessageHistory> MessageHistories => Set<MessageHistory>();
        public DbSet<MessageState> MessageStates => Set<MessageState>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints if needed
            base.OnModelCreating(modelBuilder);

            // Federation
            modelBuilder.Entity<Federation>(entity =>
            {
                entity.HasKey(m => m.Id);
            });

            // FederationContact
            modelBuilder.Entity<FederationContact>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Role).HasConversion<string>();
                entity.HasOne(m => m.Federation).WithMany(m => m.Contacts).HasForeignKey(m => m.FederationId).OnDelete(DeleteBehavior.Cascade);
            });

            // Member
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.HasOne(m => m.Federation).WithMany(m => m.Members).HasForeignKey(m => m.FederationId).OnDelete(DeleteBehavior.Cascade);
            });

            // InboxPublicCertificate
            modelBuilder.Entity<InboxPublicCertificate>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity.HasOne(m => m.Inbox).WithMany(m => m.InboxPublicCertificates).HasForeignKey(m => m.InboxId).OnDelete(DeleteBehavior.Cascade);
            });

            // Inbox
            modelBuilder.Entity<Inbox>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();
                entity.HasOne(m => m.Member).WithMany(m => m.Inboxes).HasForeignKey(m => m.MemberId).OnDelete(DeleteBehavior.Cascade);
            });

            // InboxParameter
            modelBuilder.Entity<InboxParameter>(entity =>
            {
                entity.HasKey(m => new { m.MemberId, m.InboxId });
                entity.HasOne(m => m.Member).WithMany().HasForeignKey(ip => ip.MemberId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(m => m.Inbox).WithMany(m => m.InboxParameters).HasForeignKey(m => m.MemberId).OnDelete(DeleteBehavior.Cascade);
            });
            // InboxParameter
            modelBuilder.Entity<InboxRestrictedMember>(entity =>
            {
                entity.HasKey(m => new { m.MemberId, m.InboxId });
                entity.HasOne(m => m.RestrictedMember).WithMany().HasForeignKey(ip => ip.RestrictedMemberId).OnDelete(DeleteBehavior.Restrict);
            });

            // MessageHeader
            modelBuilder.Entity<MessageHeader>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();

                entity.HasMany(m => m.MessageHistories)
                      .WithOne(h => h.MessageHeader)
                      .HasForeignKey(h => h.MessageId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade pour que l'historique soit supprimé avec le header

                entity.HasOne(m => m.MessageState)
                      .WithOne(s => s.MessageHeader)
                      .HasForeignKey<MessageState>(s => s.MessageId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade pour supprimer l'état avec le header
            });

            // MessageHistory
            modelBuilder.Entity<MessageHistory>(entity =>
            {
                entity.HasKey(h => h.Id);
                entity.Property(h => h.Id).ValueGeneratedOnAdd();
            });

            // MessageState
            modelBuilder.Entity<MessageState>(entity =>
            {
                entity.HasKey(s => s.MessageId);
            });
        }
    }
}