using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Inbox
{
    public class InboxPublicCertificateResponse
    {
        public required string Id { get; init; }
        public required string InboxId { get; init; }
        public required bool Active { get; init; }
        public required string PublicCertificatePem { get; init; }
    }
}
