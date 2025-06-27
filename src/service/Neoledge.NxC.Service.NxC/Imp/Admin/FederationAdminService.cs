using Mapster;
using Neoledge.Nxc.Domain.Api.Federation;
using Neoledge.NxC.Repository.Interfaces;
using Neoledge.NxC.Service.NxC.Interfaces;

namespace Neoledge.NxC.Service.NxC.Imp.Admin
{
    public class FederationAdminService(IFederationRepository federationRepository) : IFederationAdminService
    {
        public async Task<IList<FederationResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var federations = await federationRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return federations.Adapt<IList<FederationResponse>>();
        }
    }
}