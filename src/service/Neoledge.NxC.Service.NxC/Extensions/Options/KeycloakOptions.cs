using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.NxC.Extensions.Options
{
    public class KeycloakOptions
    {
        public required string Realm { get; init; }
        public required string ClientId { get; init; }
        public required string ClientSecret { get; init; }
        public required string BaseAdress { get; init; }
    }
    public class MinioConfig
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public bool WithSSL { get; set; }
    }
}
