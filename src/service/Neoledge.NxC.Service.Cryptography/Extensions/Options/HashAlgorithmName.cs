namespace Neoledge.NxC.Service.Cryptography.Extensions.Options
{
    /// <summary>
    /// Spécifie le nom d'un algorithme de hachage de chiffrement.
    /// </summary>
    public enum HashAlgorithmName
    {
        /// <summary>
        /// Obtient un nom d'algorithme de hachage qui représente « MD5 ».
        /// </summary>
        MD5,
        /// <summary>
        /// Obtient un nom d'algorithme de hachage qui représente « SHA1 ».
        /// </summary>
        SHA1,
        /// <summary>
        /// Obtient un nom d'algorithme de hachage qui représente « SHA256 ».
        /// </summary>
        SHA256,
        /// <summary>
        /// Obtient un nom d'algorithme de hachage qui représente « SHA384 ».
        /// </summary>
        SHA384,
        /// <summary>
        /// Obtient un nom d'algorithme de hachage qui représente « SHA512 ».
        /// </summary>
        SHA512,
        /// <summary>
        /// Obtient un HashAlgorithmName représentant « SHA3-256 ».
        /// </summary>
        SHA3_256,
        /// <summary>
        /// Obtient un HashAlgorithmName représentant « SHA3-384 ».
        /// </summary>
        SHA3_384,
        /// <summary>
        /// Obtient un HashAlgorithmName représentant « SHA3-512 ».
        /// </summary>
        SHA3_512
    }
}
