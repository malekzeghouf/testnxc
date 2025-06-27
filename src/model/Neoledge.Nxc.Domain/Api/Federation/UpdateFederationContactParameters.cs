using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Federation
{
    public class UpdateFederationContactParameters
    {
        public required int Id { get; init; }
        public FederationContactRole? Role { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Email { get; init; }
    }
}