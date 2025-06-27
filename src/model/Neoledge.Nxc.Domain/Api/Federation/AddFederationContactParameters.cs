using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Federation
{
    public class AddFederationContactParameters
    {
        public required string FederationId { get; init; }
        public required FederationContactRole Role { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
    }
}