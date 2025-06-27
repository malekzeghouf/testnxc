using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Service.ApiConnector.Extensions.Options
{
    public class ApiConnectorOptions
    {
        public required string BaseAddress { get; init; }
        public required string ClientId { get; init; }
        public required string ClientSecret { get; init; }
        public required string TokenEndpoint { get; init; }
    }
}
