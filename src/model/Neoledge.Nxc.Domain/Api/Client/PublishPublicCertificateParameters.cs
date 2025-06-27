using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Client
{
    public class PublishPublicCertificateParameters
    {
        public required  string InboxId { get; init; }
        public required string PublicCertificate { get; init; }
    }
}
