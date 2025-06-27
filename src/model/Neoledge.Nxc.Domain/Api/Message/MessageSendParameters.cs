using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Neoledge.Nxc.Domain.Api.Message
{
    public class MessageSendParameters
    {
        [Description("Todo"),MaxLength(128)]
        public required string Recipient { get; init; }
        [Description("Todo")]
        public required SenderInfoParameters SenderInfo { get; init; }
        [Description("Identifiant du message préalablement téléversé")]
        public required Guid MessageId { get; init; }
        [Description("Todo")]
        public required MessageSealParameters MessageSeal { get; init; }

    }
}
