using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Member
{
    public class UpdateMemberRequest
    {
        public required Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public bool? Enabled { get; init; }
    }
}
