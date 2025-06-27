using Neoledge.Nxc.Domain.Api.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.NxC.Interfaces
{
    public interface IMemberService
    {
        Task<IList<MemberResponse>> GetMyMembersAsync(string federationId, CancellationToken cancellationToken);
    }
}
