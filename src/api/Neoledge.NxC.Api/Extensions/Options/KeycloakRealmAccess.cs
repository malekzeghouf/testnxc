using System.Text.Json.Serialization;

namespace Neoledge.NxC.Api.Extensions.Options
{
    internal class KeycloakRealmAccess
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();
    }

}
