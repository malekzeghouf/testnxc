using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Interfaces.Api.Context
{
    public interface IUserContext
    {
        // User Identity Properties
        string UserId { get; }
        string Username { get; }
        string Email { get; }
        string Organization { get; }

        // Role Methods
        bool IsInRole(string role);
        bool HasAnyRole(params string[] roles);
        bool HasAllRoles(params string[] roles);
        IEnumerable<string> GetRealmRoles();
        IEnumerable<string> GetClientRoles();
        IEnumerable<string> GetAllRoles();

        // Permission Helpers
        bool IsSysAdmin();
        bool IsFedAdmin();
        bool IsFedAdmin(string federationId);
    }
}
