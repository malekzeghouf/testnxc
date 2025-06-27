using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neoledge.NxC.Database.Models
{
    /// <summary>
    /// Represents a contact for a member.
    /// </summary>
    public class MemberContact : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; init; }

        public required string MemberId { get; init; }

        public required string Role { get; init; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required string PhoneNumber { get; init; }

        public required string Email { get; init; }

        public Member? Member { get; init; }
    }
}