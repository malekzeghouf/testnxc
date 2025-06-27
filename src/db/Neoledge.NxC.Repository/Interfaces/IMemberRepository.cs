using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.NxC.Database.Models;

namespace Neoledge.NxC.Repository.Interfaces
{
    public interface IMemberRepository
    {
        Task<IList<Member>> GetAllAsync(string federationId, CancellationToken cancellationToken);

        Task<Member> AddMemberAsync(CreateMemberRequest createMemberRequest, CancellationToken cancellationToken);

        Task<Member> UpdateMemberAsync(UpdateMemberRequest updateMemberRequest, CancellationToken cancellationToken);

    

        Task ExistAndActiveAndBelongToFederationAsync(string federationId, string memberId, CancellationToken cancellationToken);
    }
}