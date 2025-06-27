using Neoledge.Nxc.Domain.Api.Federation;
using Neoledge.NxC.Database.Models;

namespace Neoledge.NxC.Repository.Interfaces
{
    public interface IFederationRepository
    {
        Task<IList<Federation>> GetAllAsync(CancellationToken cancellationToken);

        Task<Federation> CreateAsync(CreateFederationParameters parameters, CancellationToken cancellationToken);

        Task<Federation> UpdateAsync(UpdateFederationParameters parameters, CancellationToken cancellationToken);

        Task<Federation> AddContactAsync(AddFederationContactParameters parameters, CancellationToken cancellationToken);

        Task<FederationContact> UpdateContactAsync(UpdateFederationContactParameters parameters, CancellationToken cancellationToken);

        Task DeleteContactAsync(string id, CancellationToken cancellationToken);

        Task ExistAndActiveAsync(string federationId, CancellationToken cancellationToken);
    }
}