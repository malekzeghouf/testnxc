namespace Neoledge.Nxc.Domain.Api.Federation
{
    public class UpdateFederationParameters
    {
        public required string Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public bool? Enabled { get; init; }
    }
}