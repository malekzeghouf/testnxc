namespace Neoledge.NxC.Service.Cryptography.Message
{
    /// <summary>
    /// Sceau d'un message.
    /// </summary>
    public class MessageSceal
    {
        /// <summary>
        /// Clef de chiffrement chiffrée avec la clef publique du destinataire.
        /// </summary>
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