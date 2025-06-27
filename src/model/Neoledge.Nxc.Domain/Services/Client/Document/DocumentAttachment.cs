using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Services.Client.Document
{
    public class DocumentAttachment
    {
        public required string Description { get; init; }

        public required string FileName { get; init; }

        public required bool IsConfidential { get; init; }

        public required int Position { get; init; }
    }
}
