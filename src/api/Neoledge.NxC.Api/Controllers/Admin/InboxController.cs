using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neoledge.Nxc.Domain.Api.Inbox;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.NxC.Repository.Imp;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Tags("Admin.Inbox")]
    public class InboxController(IInboxRepository inboxRepository) : ControllerBase
    {
        [HttpGet("/certificates")]
        [EndpointSummary("Get certificates")]
        [EndpointDescription("Get inbox certificates")]
        [ProducesResponseType(typeof(IList<InboxPublicCertificateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCertificates(string inboxId, CancellationToken cancellationToken)
        {
            var inbox = await inboxRepository.GetInboxCertificatesAsync(inboxId, cancellationToken);
            var response = inbox.Adapt<IList<InboxPublicCertificateResponse>>();
            return Ok(response);
        }

        [HttpPost("/certificate")]
        [EndpointSummary("Generate certificate")]
        [EndpointDescription("Generate new inbox certificate")]
        [ProducesResponseType(typeof(IList<InboxPublicCertificateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GenerateCertificate(string inboxId, CancellationToken cancellationToken)
        {
            var inbox = await inboxRepository.GetInboxCertificatesAsync(inboxId, cancellationToken);
            var response = inbox.Adapt<IList<InboxPublicCertificateResponse>>();
            return Ok(response);
        }

        [HttpGet("/certificate/import")]
        [EndpointSummary("Import certificate")]
        [EndpointDescription("Import public certificate")]
        [ProducesResponseType(typeof(IList<InboxPublicCertificateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ImportPem(string inboxId, CancellationToken cancellationToken)
        {
            var inbox = await inboxRepository.GetInboxCertificatesAsync(inboxId, cancellationToken);
            var response = inbox.Adapt<IList<InboxPublicCertificateResponse>>();
            return Ok(response);
        }

        [HttpPut("/certificate")]
        [EndpointSummary("Update certificate")]
        [EndpointDescription("Get inbox certificates")]
        [ProducesResponseType(typeof(IList<InboxPublicCertificateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCertificate(string inboxId, CancellationToken cancellationToken)
        {
            var inbox = await inboxRepository.GetInboxCertificatesAsync(inboxId, cancellationToken);
            var response = inbox.Adapt<IList<InboxPublicCertificateResponse>>();
            return Ok(response);
        }
    }
}