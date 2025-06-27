namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Spécifie le mode utilisé pour le contrôle de révocation du certificat X509.
    /// </summary>
    public enum RevocationMode
    {
        /// <summary>
        /// Aucun contrôle de révocation n'est effectué sur le certificat.
        /// </summary>
        NoCheck,
        /// <summary>
        /// Un contrôle de révocation est effectué à l'aide d'une liste de révocation de certificats mise en cache.
        /// </summary>
        Online,
        /// <summary>
        /// Un contrôle de révocation est effectué à l'aide d'une liste de révocation de certificats en ligne.
        /// </summary>
        Offline
    }
}
