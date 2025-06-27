using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Message
{
    public class MessageStateUpateParameters
    {
        public required Guid MessageId { get; init; }
        public int? StatusCode { get; init; }
        public string? StatusMessage { get; init; }
        public string? LocalIdentifier { get; init; }
    }
}