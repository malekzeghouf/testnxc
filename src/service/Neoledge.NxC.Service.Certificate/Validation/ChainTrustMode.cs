namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Mode déterminant l’approbation racine pour la création de la chaîne de certificats.
    /// </summary>
    public enum ChainTrustMode
    {
        /// <summary>
        /// Lorsque cette valeur est utilisée, CustomTrustStore est utilisé à la place de l’approbation racine par défaut.
        /// </summary>
        CustomRootTrust,

        /// <summary>
        /// Utilisez l’approbation racine (système) par défaut.
        /// </summary>
        System
    }
}