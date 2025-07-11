﻿namespace Neoledge.Nxc.Domain.Api.Federation
{
    public class FederationResponse
    {
        public required string Id { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}