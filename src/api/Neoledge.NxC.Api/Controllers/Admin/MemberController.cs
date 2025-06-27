using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "sysadmin")]
    [Tags("Admin/Member")]
    public class MemberController(IMemberRepository memberRepository) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("List members")]
        [EndpointDescription("List members of a federation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(string federationId, CancellationToken cancellationToken)
        {
            var members = await memberRepository.GetAllAsync(federationId, cancellationToken);
            return Ok(members.Adapt<IList<MemberResponse>>());
        }

        [HttpPost]
        [EndpointSummary("Add member")]
        [EndpointDescription("Add member to a federation")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateMemberRequest request, CancellationToken cancellationToken)
        {
            var federation = await memberRepository.AddMemberAsync(request, cancellationToken);
            var response = federation.Adapt<MemberResponse>();
            return CreatedAtAction(nameof(Create), new { id = federation.Id }, response);
        }

        [HttpPut]
        [EndpointSummary("Update member")]
        [EndpointDescription("Update member")]
        [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(UpdateMemberRequest request, CancellationToken cancellationToken)
        {
            var member = await memberRepository.UpdateMemberAsync(request, cancellationToken);
            var response = member.Adapt<MemberResponse>();
            return Ok(response);
        }



       
    }
}