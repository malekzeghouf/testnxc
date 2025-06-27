using Mapster;
using Microsoft.EntityFrameworkCore;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.Nxc.Domain.Exceptions;
using Neoledge.NxC.Database;
using Neoledge.NxC.Database.Models;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Repository.Imp
{
    public class MemberRepository(IAppDbContext context) : IMemberRepository
    {
        private async Task<bool> MemberExistsAsync(string memberId, string federationId, CancellationToken cancellationToken)
        {
            return await context.Members.AnyAsync(m => m.Id == memberId && m.FederationId == federationId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Member> AddMemberAsync(CreateMemberRequest createMemberRequest, CancellationToken cancellationToken)
        {
            // Check if Federation exists
            var exist = await context.Federations.AsNoTracking()
                .AnyAsync(f => f.Id == createMemberRequest.FederationId, cancellationToken)
                .ConfigureAwait(false);

            if (!exist)
                throw new EntityNotFoundException(nameof(Federation), createMemberRequest.FederationId);

            // Check if Member exists
            if (await MemberExistsAsync(createMemberRequest.Name, createMemberRequest.FederationId, cancellationToken).ConfigureAwait(false))
                throw new EntityValidationException("Member already exists in this federation.");

            var member = createMemberRequest.Adapt<Member>();

            context.Members.Add(member);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return member;
        }

        public Task<Member> UpdateMemberAsync(UpdateMemberRequest updateMemberRequest, CancellationToken cancellationToken)
        {
            // TODO: Implement UpdateMemberAsync method
            throw new NotImplementedException();
        }

        public async Task<IList<Member>> GetAllAsync(string federationId, CancellationToken cancellationToken)
        {
            var members = await context.Members//.Include(m => m.Federation)
                .AsNoTracking()
                .Where(m => m.FederationId == federationId)
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return members;
        }

        public async Task ExistAndActiveAndBelongToFederationAsync(string federationId, string memberId, CancellationToken cancellationToken)
        {
            var member = await context.Members.AsNoTracking().Include(m => m.Federation)
                                                       .FirstOrDefaultAsync(m => m.Id == memberId && m.FederationId == federationId, cancellationToken)
                                                       .ConfigureAwait(false)
                                                       ?? throw new EntityNotFoundException(nameof(Member), memberId);
            if (!member.Enabled)
                throw new EntityValidationException("Member is not active.");
        }
    }
}