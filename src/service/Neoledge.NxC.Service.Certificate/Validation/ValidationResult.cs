
namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Résultat de la validation
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Identifiant de la politique utilisée pour la validation
        /// </summary>
        public required string PolicyIdentifier { get; init; }
        /// <summary>
        /// Etat de validation du certificat
        /// </summary>
        public required bool Validated {  get; init; }
        /// <summary>
        /// Message de rapport de validation 
        /// </summary>
        public required string Message { get; init; }
    }
}
