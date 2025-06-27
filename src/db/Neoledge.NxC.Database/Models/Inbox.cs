using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Database.Models
{
    /// <summary>
    /// Represents a member's inbox.
    /// </summary>
    public class Inbox : BaseEntity
    {
        public required string Id { get; init; }

        public required string MemberId { get; init; }

        public required string Name { get; init; }

        public string? Description { get; init; }

        public bool Enabled { get; init; }

        public required Member Member { get; init; }

        public ICollection<InboxPublicCertificate> InboxPublicCertificates { get; init; } = [];

        public ICollection<InboxParameter> InboxParameters { get; init; } = [];
        public ICollection<InboxRestrictedMember> InboxRestrictedMembers { get; init; } = [];
    }
}
