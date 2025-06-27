using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Client
{
    public class SendParameters
    {
        public required string ReceiverInboxId { get; init; }
        public required string ReceiverMemberId { get; init; }
        public required string ReceiverFederationId { get; init; }
        public required string SenderInboxId { get; init; }
    }
}
