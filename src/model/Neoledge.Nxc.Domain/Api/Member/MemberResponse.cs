namespace Neoledge.Nxc.Domain.Api.Member
{
    public class MemberResponse
    {
        public required string Id { get; init; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Enabled { get; set; }
    }
}