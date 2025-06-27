namespace Neoledge.NxC.Database.Models
{
    /// <summary>
    /// Represents a registered member in the system.
    /// </summary>
    public class Member : BaseEntity
    {
        public required string Id { get; init; }

        public required string FederationId { get; init; }

        public required string Name { get; init; }

        public string? Description { get; init; }

        public bool Enabled { get; init; }

        public Federation? Federation { get; init; }

        public ICollection<Inbox> Inboxes { get; init; } = [];
    }
}