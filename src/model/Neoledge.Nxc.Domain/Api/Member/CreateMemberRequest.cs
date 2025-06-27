namespace Neoledge.Nxc.Domain.Api.Member
{
    public class CreateMemberRequest
    {
        public required string FederationId { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
    }
}