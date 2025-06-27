namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Spécifie les conditions dans lesquelles la vérification des certificats de la chaîne X509 doit être effectuée.
    /// </summary>
    [Flags]
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
    public enum VerificationFlags
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
    {
        /// <summary>
        /// Tous les indicateurs liés à la vérification sont inclus.
        /// </summary>
        AllFlags = 4095,

        /// <summary>
        /// Ignore que la chaîne ne peut pas être vérifiée en raison d’une autorité de certification inconnue ou de chaînes partielles.
        /// </summary>
        AllowUnknownCertificateAuthority = 16,

        /// <summary>
        /// Ignore que la révocation de l'autorité de certification est inconnue lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreCertificateAuthorityRevocationUnknown = 1024,

        /// <summary>
        ///Ignore que la liste de certificats de confiance (CTL, Certificate Trust List) n'est pas valide, pour des raisons telles que l'expiration de la liste CTL, lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreCtlNotTimeValid = 2,

        /// <summary>
        /// Ignore que la révocation du signataire de la liste de certificats de confiance (CTL, Certificate Trust List) est inconnue lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreCtlSignerRevocationUnknown = 512,

        /// <summary>
        /// Ignore que la révocation du certificat (utilisateur) final est inconnue lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreEndRevocationUnknown = 256,

        /// <summary>
        /// Ignore que les contraintes de base ne sont pas valides lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreInvalidBasicConstraints = 8,

        /// <summary>
        /// Ignore que le certificat a un nom qui n'est pas valide lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreInvalidName = 64,

        /// <summary>
        /// Ignore que le certificat a une stratégie qui n'est pas valide lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreInvalidPolicy = 128,

        /// <summary>
        /// Ignore que le certificat de l'autorité de certification et que le certificat émis ont des périodes de validité qui ne sont pas imbriquées lors de la vérification du certificat. Par exemple, le certificat d'autorité de certification peut être valide du 1er janvier au 1er décembre et le certificat émis du 2 janvier au 2 décembre, ce qui signifie que les périodes de validité ne sont pas imbriquées.
        /// </summary>
        IgnoreNotTimeNested = 4,

        /// <summary>
        /// Ignore les certificats de la chaîne qui ne sont pas valides soit parce qu'ils ont expiré, soir parce qu'ils ne sont pas encore en vigueur lors de la détermination de la validité du certificat.
        /// </summary>
        IgnoreNotTimeValid = 1,

        /// <summary>
        /// Ignore que la révocation de la racine est inconnue lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreRootRevocationUnknown = 2048,

        /// <summary>
        /// Ignore que le certificat n'a pas été émis pour son utilisation actuelle lors de la détermination de la vérification du certificat.
        /// </summary>
        IgnoreWrongUsage = 32,

        /// <summary>
        /// Aucun indicateur lié à la vérification n'est inclus.
        /// </summary>
        None = 0,
    }
}