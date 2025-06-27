
namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Indique les certificats X509 de la chaîne dont la révocation doit être vérifiée.
    /// </summary>
    public enum RevocationFlag
    {
        /// <summary>
        /// Seul le certificat final fait l'objet d'un contrôle de révocation.
        /// </summary>
        EndCertificateOnly,
        /// <summary>
        /// L'ensemble de la chaîne de certificats est vérifié en vue d'une éventuelle révocation.
        /// </summary>
        EntireChain,
        /// <summary>
        /// L'ensemble de la chaîne, à l'exception du certificat racine, fait l'objet d'un contrôle de révocation.
        /// </summary>
        ExcludeRoot
    }
}
