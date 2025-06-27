using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.Nxc.Domain.Api.Message
{
    public class MessageSealParameters
    {
        public required byte[] EncryptedKey { get; init; }

        /// <summary>
        /// Vecteur d'initialisation chiffré avec la clef publique du destinataire.
        /// </summary>
        public required byte[] EncryptedIv { get; init; }

        /// <summary>
        /// Empreinte du corps du message chiffré avec la clef privée de l'expéditeur.
        /// </summary>
        public required byte[] SignedFileHash { get; init; }
    }
}