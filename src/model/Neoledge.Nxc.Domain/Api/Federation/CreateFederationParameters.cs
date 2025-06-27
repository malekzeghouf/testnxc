namespace Neoledge.Nxc.Domain.Api.Federation
{
    public class CreateFederationParameters
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public bool Enabled { get; init; }
    }
}