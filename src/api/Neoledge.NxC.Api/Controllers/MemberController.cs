using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.Nxc.Domain.Interfaces.Api.Context;
using Neoledge.NxC.Service.NxC.Interfaces;

namespace Neoledge.NxC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Member")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MemberController(IMemberService memberService, IUserContext userContext) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("List members")]
        [EndpointDescription("List members of my federation")]
        [ProducesResponseType(typeof(IList<MemberResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            return Ok(await memberService.GetMyMembersAsync(federationId, cancellationToken));
        }

        
    }
}