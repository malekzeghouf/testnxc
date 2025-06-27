namespace Neoledge.NxC.Service.Cryptography.Message
{
    /// <summary>
    /// Résultat da la vérification d'un message.
    /// </summary>
    public class MessageVerificationResult
    {
        /// <summary>
        /// Code d'état de la vérification.
        /// </summary>
        public required VerificationResultCode VerificationResultCode { get; init; }
        /// <summary>
        /// Message descriptif.
        /// </summary>
        public string? Message { get; init; }
        /// <summary>
        /// Exception éventuelle.
        /// </summary>
        public Exception? Exception { get; init; }
    }
}