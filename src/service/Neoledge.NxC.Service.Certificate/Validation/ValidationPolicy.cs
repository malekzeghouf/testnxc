using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Politique de validation des certificats
    /// </summary>
    public class ValidationPolicy
    {
        /// <summary>
        /// Identifiant de la politique de validation
        /// </summary>
        public required string PolicyIdentifier { get; init; }

        /// <summary>
        /// Désactive intégralement les contrôles.
        /// C'est dangereux et à utiliser uniquement en développement.
        /// </summary>
        public bool DisableAllValidation { get; init; }

        /// <summary>
        /// Spécifie le mode utilisé pour le contrôle de révocation du certificat X509.
        /// </summary>
        public RevocationMode RevocationMode { get; init; } = RevocationMode.Offline;

        /// <summary>
        /// Spécifie le mode utilisé pour le contrôle de révocation du certificat X509.
        /// </summary>
        public RevocationFlag RevocationFlag { get; init; } = RevocationFlag.ExcludeRoot;

        /// <summary>
        /// Spécifie les conditions dans lesquelles la vérification des certificats de la chaîne X509 doit être effectuée.
        /// </summary>
        public VerificationFlags VerificationFlags { get; init; } = VerificationFlags.None;

        /// <summary>
        /// Mode déterminant l’approbation racine pour la création de la chaîne de certificats.
        /// </summary>
        public ChainTrustMode ChainTrustMode { get; init; } = ChainTrustMode.System;

        /// <summary>
        /// Collection d’identificateurs d’objet (OID) qui spécifie quelles stratégies d’application ou utilisations de clé améliorée (EKU) doivent être prises en charge par le certificat.
        /// </summary>
        public IList<string> ApplicationPolicy { get; } = [];

        /// <summary>
        /// Collection d'identifiants d'objets (OID) spécifiant les politiques de certification que le certificat doit prendre en charge.
        /// </summary>
        public IList<string> CertificatePolicy { get; } = [];

        /// <summary>
        /// Obtient ou définit une valeur qui indique si le moteur de chaîne peut utiliser l’extension AIA (Authority Information Access) pour localiser les certificats d’émetteur inconnus.
        /// </summary>
        public bool DisableCertificateDownloads { get; init; }

        /// <summary>
        /// Obtient ou définit le temps maximal qui doit être passé pendant la vérification de révocation en ligne ou le téléchargement de la liste de révocation de certificats (CRL). La valeur Zero signifie aucune limite.
        /// </summary>
        public TimeSpan UrlRetrievalTimeout { get; init; }  = TimeSpan.Zero;

        /// <summary>
        /// Obtient ou définit une valeur qui indique si la validation de la chaîne doit utiliser VerificationTime ou l’heure système actuelle lors de la création d’une chaîne de certificats X.509.
        /// </summary>
        /// <remarks>
        /// true si VerificationTime est ignoré et que l’heure système actuelle est utilisée ; sinon false. Par défaut, il s’agit de true.
        /// </remarks>
        public bool VerificationTimeIgnored { get; init; } = true;

        /// <summary>
        /// Obtient ou définit l’heure à laquelle la chaîne doit avoir été validée.
        /// </summary>
        public DateTime VerificationTime { get; init; }

        /// <summary>
        /// Représente une collection de certificats qui remplacent la confiance dans le certificat par défaut.
        /// </summary>
        /// <remarks>
        /// La collection est utilisée uniquement lorsque TrustMode est défini sur CustomRootTrust.
        /// </remarks>
        public X509Certificate2Collection CustomTrustStore { get; } = new X509Certificate2Collection();
    }
}