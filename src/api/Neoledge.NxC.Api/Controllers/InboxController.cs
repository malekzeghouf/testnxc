using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neoledge.Nxc.Domain.Api.Client;
using Neoledge.Nxc.Domain.Interfaces.Api.Context;
using Neoledge.NxC.Service.NxC.Interfaces;

namespace Neoledge.NxC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Inbox")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InboxController(IUserContext userContext, IInboxService inboxService) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("List my inbox")]
        [EndpointDescription("List my inboxes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListMy(CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            var memberId = userContext.Username;
            return Ok(await inboxService.GetMyInboxesAsync(federationId, memberId, cancellationToken).ConfigureAwait(false));
        }

        [HttpGet("{memberId}")]
        [EndpointSummary("List inbox")]
        [EndpointDescription("List inbox of member")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(string memberId, CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            var forMemberId = userContext.Username;
            return Ok(await inboxService.GetInboxesForMemberAsync(federationId, memberId, forMemberId, cancellationToken).ConfigureAwait(false));
        }

        [HttpGet("certificate/{inboxId}")]
        [EndpointSummary("Get public certificate")]
        [EndpointDescription("Get inbox public certificate")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetInboxPublicCertificate(string inboxId, CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            return Ok(await inboxService.GetInboxPublicCertificateAsync(federationId, inboxId, cancellationToken));
        }

        [HttpPost("certificate")]
        [EndpointSummary("Publish public certificate")]
        [EndpointDescription("Publish inbox public certificate")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PublishPublicCertificate(PublishPublicCertificateParameters parameters, CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            var memberId = userContext.Username;
            return Ok(await inboxService.PublishPublicCertificateAsync(federationId, memberId, parameters.InboxId, parameters.PublicCertificate, cancellationToken));
        }
    }
}