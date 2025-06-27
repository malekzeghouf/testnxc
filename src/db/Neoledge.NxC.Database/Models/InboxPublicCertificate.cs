using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Database.Models
{
    /// <summary>
    /// Represents a public key associated with a inbox.
    /// </summary>
    public class InboxPublicCertificate : BaseEntity
    {
        public int Id { get; init; }

        public required string InboxId { get; init; }

        public bool Active { get; set; }

        public required string PublicCertificatePem { get; init; }

        public Inbox? Inbox { get; init; }
    }
}