using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neoledge.Nxc.Domain.Api.Federation;
using Neoledge.NxC.Repository.Interfaces;
using Neoledge.NxC.Service.NxC.Interfaces;

namespace Neoledge.NxC.Api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "sysadmin")]
    [Tags("Admin.Federation")]
    public class FederationController(IFederationRepository federationRepository, IFederationAdminService federationAdminService) : ControllerBase
    {
        [HttpGet]
        [EndpointSummary("List federations")]
        [EndpointDescription("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var federations = await federationAdminService.GetAllAsync(cancellationToken).ConfigureAwait(false);
            return Ok(federations);
        }

        [HttpPost]
        [EndpointSummary("Create federation")]
        [EndpointDescription("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateFederationParameters parameters, CancellationToken cancellationToken)
        {
            var federation = await federationRepository.CreateAsync(parameters, cancellationToken).ConfigureAwait(false);
            var response = federation.Adapt<FederationResponse>();
            return CreatedAtAction(nameof(Create), new { id = federation.Id }, response);
        }

        [HttpPut]
        [EndpointSummary("Update federation")]
        [EndpointDescription("")]
        [ProducesResponseType(typeof(FederationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(UpdateFederationParameters parameters, CancellationToken cancellationToken)
        {
            var federation = await federationRepository.UpdateAsync(parameters, cancellationToken).ConfigureAwait(false);
            var response = federation.Adapt<FederationResponse>();
            return Ok(response);
        }

        [HttpPost("contact")]
        [EndpointSummary("Add contact federation")]
        [EndpointDescription("")]
        [ProducesResponseType(typeof(FederationContactResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddContact(AddFederationContactParameters parameters, CancellationToken cancellationToken)
        {
            var federation = await federationRepository.AddContactAsync(parameters, cancellationToken).ConfigureAwait(false);
            var response = federation.Adapt<FederationContactResponse>();
            return Ok(response);
        }

        [HttpPut("contact")]
        [EndpointSummary("Update contact federation")]
        [EndpointDescription("")]
        [ProducesResponseType(typeof(FederationContactResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateContact(UpdateFederationContactParameters parameters, CancellationToken cancellationToken)
        {
            var federation = await federationRepository.UpdateContactAsync(parameters, cancellationToken).ConfigureAwait(false);
            var response = federation.Adapt<FederationContactResponse>();
            return Ok(response);
        }
    }
}