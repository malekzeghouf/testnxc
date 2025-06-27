using Mapster;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.NxC.Repository.Imp;
using Neoledge.NxC.Repository.Interfaces;
using Neoledge.NxC.Service.NxC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.NxC.Imp
{
    public class MemberService(IMemberRepository memberRepository, IFederationRepository federationRepository) : IMemberService
    {
        public Task<IList<MemberResponse>> GetMyMembersAsync(string federationId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            return GetMyMembersInternalAsync(federationId, cancellationToken);
        }

        private async Task<IList<MemberResponse>> GetMyMembersInternalAsync(string federationId, CancellationToken cancellationToken)
        {
            await federationRepository.ExistAndActiveAsync(federationId, cancellationToken).ConfigureAwait(false);
            var members = await memberRepository.GetAllAsync(federationId, cancellationToken).ConfigureAwait(false);
            return members.Adapt<IList<MemberResponse>>();
        }
    }
}