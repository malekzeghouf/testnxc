using FS.Keycloak.RestApiClient.Model;
using Neoledge.Nxc.Domain.Api.Federation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.NxC.Interfaces
{
    public interface IFederationAdminService
    {
        Task<IList<FederationResponse>> GetAllAsync(CancellationToken cancellationToken);
    }
}
