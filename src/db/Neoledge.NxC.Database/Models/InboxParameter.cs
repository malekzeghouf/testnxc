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
    /// Parameters associated with an Inbox.
    /// </summary>
    public class InboxParameter : BaseEntity
    {
        public required string InboxId { get; init; }

        public required string MemberId { get; init; }

        public required string Name { get; init; }

        public required string Value { get; init; }

        public required Inbox Inbox { get; init; }

        public required Member Member { get; init; }
    }
}