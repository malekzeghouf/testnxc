using Neoledge.Nxc.Domain.Api.Federation;

namespace Neoledge.NxC.Database.Models
{
    public class FederationContact : BaseEntity
    {
        public required int Id { get; init; }

        public required string FederationId { get; init; }

        public required FederationContactRole Role { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public required Federation Federation { get; init; }
    }
}