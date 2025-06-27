using System.Text.Json.Serialization;

namespace Neoledge.NxC.Api.Extensions.Options
{
    internal class KeycloakClientAccess
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();
    }
}
