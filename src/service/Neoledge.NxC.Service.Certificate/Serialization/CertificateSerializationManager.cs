using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Certificate.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Certificate.Serialization
{
    public class CertificateSerializationManager(IOptions<CertificateServiceOptions> options) : ICertificateSerializationManager
    {
        /// <summary>
        /// Exporte le certificat X.509 public, encodé en PEM.
        /// </summary>
        /// <param name="certificate">Le certificat à exporter.</param>
        /// <remark>
        /// Un certificat X.509 codé en PEM commence par -----BEGIN CERTIFICATE----- et se termine par -----END CERTIFICATE-----
        /// , avec le contenu DER codé en base64 du certificat entre les limites PEM. Le certificat est encodé selon les règles d’encodage
        /// « strictes » RFC 7468 de l’IETF.
        /// </remark>
        /// <returns>Encodage PEM du certificat.</returns>
        public string ExportCertificatePem(X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(certificate);
            return certificate.ExportCertificatePem();
        }

        /// <summary>
        /// Exporte la clé actuelle au format PKCS#8 EncryptedPrivateKeyInfo avec un mot de passe pem encodé.
        /// </summary>
        /// <param name="certificate">Le certificat donc la clé est à exporter.</param>
        /// <param name="password">Le mot de passe pour encoder la clef.</param>
        /// <returns>Chaîne contenant le PKCS#8 EncryptedPrivateKeyInfo codé en PEM.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public string ExportPkcs8PrivateKeyPem(X509Certificate2 certificate, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            ArgumentNullException.ThrowIfNull(certificate);
            if (password.Length < options.Value.PbeMinimumPasswordLength) throw new ArgumentException($"The password does not have the minimum required length {options.Value.PbeMinimumPasswordLength} defined in the configuration.", password);
            if (!certificate.HasPrivateKey) throw new ArgumentException($"The certificate {certificate.Subject} ({certificate.SerialNumber}) does not have a private key.", nameof(certificate));
            var key = certificate.GetRSAPrivateKey() ?? throw new InvalidOperationException($"Certificate {certificate.Subject} ({certificate.SerialNumber}) does not have an RSA private key.");
            return key.ExportEncryptedPkcs8PrivateKeyPem(password, GetPbeParameters());
        }

        /// <summary>
        /// Crée un certificat X509 public à partir du contenu d’un certificat encodé en PEM RFC 7468.
        /// </summary>
        /// <param name="certificatePem"></param>
        /// <returns></returns>
        public X509Certificate2 CreateFromPem(string certificatePem)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(certificatePem);
            return X509Certificate2.CreateFromPem(certificatePem);
        }

        /// <summary>
        /// Crée un certificat x509 à partir du contenu d’un certificat RFC 7468 encodé PEM et de la clé privée protégée par mot de passe.
        /// </summary>
        /// <param name="certificatePem"></param>
        /// <param name="keyPem"></param>
        /// <returns></returns>
        public X509Certificate2 CreateFromEncryptedPem(string certificatePem, string keyPem, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(certificatePem);
            ArgumentException.ThrowIfNullOrWhiteSpace(keyPem);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            return X509Certificate2.CreateFromEncryptedPem(certificatePem, keyPem, password);
        }

        /// <summary>
        /// Ouvre le fichier spécifié, lit le contenu sous la forme d’un PFX PKCS#12 et extrait un certificat.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public X509Certificate2 LoadPkcs12FromFile(string fileName, string password)
        {
            return X509CertificateLoader.LoadPkcs12FromFile(fileName, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
        }

        private PbeParameters? _pbeParameters = null;

        private PbeParameters GetPbeParameters()
        {
            _pbeParameters ??= new PbeParameters(options.Value.PbeEncryptionAlgorithm.Convert()
                                                    , options.Value.PbeHashAlgorithmName.Convert()
                                                    , options.Value.PbeIterationCount);
            return _pbeParameters;
        }
    }
}