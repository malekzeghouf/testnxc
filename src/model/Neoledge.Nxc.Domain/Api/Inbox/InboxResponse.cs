using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Inbox
{
    public class InboxResponse
    {
        public required string Id { get; init; }
        public required string MemberId { get; init; }
        public required string Name { get; init; }
        public  string? Description { get; init; }
        public  bool Enabled { get; init; }
    }
}
