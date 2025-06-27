using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Neoledge.Nxc.Domain.Api.Message
{
    public class SenderInfoParameters
    {
        [Description("Cn de l'utilisateur à l'origine de l'action"), MaxLength(128)]
        public required string UserCn { get; init; }

        [Description("Id de l'utilisateur à l'origine de l'action"), MaxLength(128)]
        public required string UserId { get; init; }

        [Description("Id de l'inbox à l'origine de l'action"), MaxLength(128)]
        public required string SenderInboxId { get; init; }
    }
}