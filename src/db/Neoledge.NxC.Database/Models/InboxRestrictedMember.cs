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
    /// Restricts inbox access to specific members.
    /// </summary>
    public class InboxRestrictedMember : BaseEntity
    {

        public required string InboxId { get; init; }

        public required string MemberId { get; init; }

        public required string RestrictedMemberId { get; init; }

        public required Member RestrictedMember { get; init; }

    }

}
