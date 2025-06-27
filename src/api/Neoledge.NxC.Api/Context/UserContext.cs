using Microsoft.Extensions.Options;
using Neoledge.Nxc.Domain.Interfaces.Api.Context;
using Neoledge.NxC.Service.NxC.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace Neoledge.NxC.Api.Context
{
    public class UserContext(IHttpContextAccessor httpContextAccessor, IOptions<KeycloakOptions> keycloakOptions) : IUserContext
    {
        public string UserId => GetClaimValue(ClaimTypes.Sid);
        public string Username => isServiceAccount ? GetClaimValue("azp") : GetClaimValue("preferred_username");
        public string Email => GetClaimValue("email");
        public string Organization => GetClaimValue("organization");
        private bool isServiceAccount => GetClaimValue("azp") != keycloakOptions.Value.ClientId;

        public bool IsInRole(string role) => GetAllRoles().Contains(role);

        public bool HasAnyRole(params string[] roles) => roles.Any(role => GetAllRoles().Contains(role));

        public bool HasAllRoles(params string[] roles) => roles.All(role => GetAllRoles().Contains(role));

        public IEnumerable<string> GetRealmRoles()
        {
            var realmAccess = GetClaimJson("realm_access");
            return realmAccess?.GetProperty("roles").EnumerateArray()
                .Select(r => r.GetString() ?? string.Empty) ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetClientRoles()
        {
            var resourceAccess = GetClaimJson("resource_access");
            if (resourceAccess?.TryGetProperty(keycloakOptions.Value.ClientId, out var clientAccess) == true)
            {
                return clientAccess.GetProperty("roles").EnumerateArray()
                    .Select(r => r.GetString() ?? string.Empty) ?? Enumerable.Empty<string>();
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetAllRoles() => GetRealmRoles().Concat(GetClientRoles());

        public bool IsSysAdmin() => HasAnyRole("sysadmin");

        public bool IsFedAdmin() => HasAnyRole("fedadmin");

        public bool IsFedAdmin(string federationId) => HasAnyRole("fedadmin") && Organization == federationId;

        // Helper methods
        private string GetClaimValue(string claimType) => httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value ?? string.Empty;

        private JsonElement? GetClaimJson(string claimType)
        {
            var claim = httpContextAccessor.HttpContext?.User?
                .FindFirst(claimType);

            if (claim == null || string.IsNullOrEmpty(claim.Value))
                return null;

            try
            {
                return JsonSerializer.Deserialize<JsonElement>(claim.Value);
            }
            catch
            {
                return null;
            }
        }
    }
}