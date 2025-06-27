namespace Neoledge.NxC.Service.Cryptography.Message
{
    /// <summary>
    /// Code d'état de la vérification d'un message.
    /// </summary>
    public enum VerificationResultCode
    {
        /// <summary>
        /// Ok, le message est intègre.
        /// </summary>
        Ok = 0,
        /// <summary>
        /// La vérification de l'empreinte du corps du message avec la signature du destinataire a échoué.
        /// </summary>
        FailedHashVerification,
        /// <summary>
        /// Erreur innatendue.
        /// </summary>
        UnexpectedError,
    }
}