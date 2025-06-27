using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neoledge.Nxc.Domain.Api.Message;
using Neoledge.Nxc.Domain.Interfaces.Api.Context;
using Neoledge.NxC.Api.Context;
using Neoledge.NxC.Service.NxC.Interfaces;
using System.Net.Mime;

namespace Neoledge.NxC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Message")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageController(ILogger<MessageController> logger, IMessageService messageService, IUserContext userContext) : ControllerBase
    {
        [HttpPost("Send/Body")]
        [EndpointSummary("Déposer un message")]
        [EndpointDescription("Permet de déposer le corps d'un message, format binaire.")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(SendBodyResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        public async Task<IActionResult> SendBody(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var objectName = await messageService.SendBodyAsync(file.OpenReadStream(), file.ContentType, CancellationToken.None).ConfigureAwait(false);

            return Ok(new SendBodyResponse() { Guid = objectName });
        }

        [HttpPost("Send")]
        [EndpointSummary("Envoyer un message")]
        [EndpointDescription("Méthode permettant remettre un message à envoyer.")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(SendMessageResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Send([FromBody] MessageSendParameters sendRequest, CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            var senderMemberId = userContext.Username;
            var senderInboxId = sendRequest.SenderInfo.SenderInboxId;
            var messageId = await messageService.SendAsync(sendRequest, senderInboxId, senderMemberId, federationId, cancellationToken).ConfigureAwait(false);
            return Ok(new SendMessageResponse() { Id = messageId });
        }

        [HttpGet("Peek")]
        [EndpointSummary("consulter le dernier message")]
        [EndpointDescription("Permet de retourner le dernier message ")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        public async Task<IActionResult> PeekMessage(CancellationToken cancellationToken)
        {
            var federationId = userContext.Organization;
            var memberId = userContext.Username;
            var messageId = await messageService.PeekMessageAsync(memberId, federationId, cancellationToken).ConfigureAwait(false);
            return Ok(new PeekMessageResponse() { MessageId = messageId });
        }

        [HttpGet("Receive/Body/{messageId}")]
        [EndpointSummary("Télécharger le corps d'un message")]
        [EndpointDescription("Permet de télécharger le contenu binaire d'un message spécifique.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMessageBody(Guid messageId, CancellationToken cancellationToken)
        {
            var file = new MemoryStream();
            await messageService.GetMessageBodyAsync(messageId, file, cancellationToken).ConfigureAwait(false);

            if (file == null)
            {
                return NotFound($"Message with ID '{messageId}' not found or its content could not be retrieved.");
            }

            // Content-Type générique pour données binaires (ZIP chiffré sans extension)
            return File(file, MediaTypeNames.Application.Octet);
        }

        [HttpGet("Receive/{messageId}")]
        [EndpointSummary("Télécharger un message")]
        [EndpointDescription("Permet de télécharger le dernier message")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMessage([FromRoute] Guid messageId, CancellationToken cancellationToken)
        {
            var msg = await messageService.ReceivedMessageAsync(messageId, cancellationToken).ConfigureAwait(false);
            return Ok(msg);
        }

        [HttpPut("Receive/Ack")]
        [EndpointSummary("faire le Ack par l'automate")]
        [EndpointDescription("le message a bien été intégré par le destinataire")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> IntegrateMessage(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            logger.LogInformation("le message a bien été intégré par le destinataire ");
            return Ok(await messageService.AckMessageAsync(parameters, cancellationToken).ConfigureAwait(false));
        }

        [HttpPut("Receive/Nak")]
        [EndpointSummary("faire le Nak par l'automate")]
        [EndpointDescription("le message n'a pas été intégré par le destinataire")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> NakMessage(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            logger.LogInformation("le message n'a pas été intégré par le destinataire ");
            return Ok(await messageService.NaKMessageAsync(parameters, cancellationToken).ConfigureAwait(false));
        }

        [HttpPut("Receive/Reject")]
        [EndpointSummary("faire le Nak par l'automate")]
        [EndpointDescription("le message n'a pas été intégré par le destinataire")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RejectMessage(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            logger.LogInformation("le message a été rejeté par le destinataire ");
            return Ok(await messageService.RejectMessageAsync(parameters, cancellationToken).ConfigureAwait(false));
        }
    }
}