using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Cipher;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Hash;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.SessionKey;
using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Cryptography.Message
{
    /// <summary>
    /// Gestion des messages : préparation, vérification et réception
    /// </summary>
    public class MessageManager(ISessionKeyManager sessionKeyManager
                                , ISymmetricCipherManager symmetricCipherManager
                                , IHashManager hashManager
                                , IAsymmetricCipherManager asymmetricCipherManager) : IMessageManager
    {
        /// <summary>
        /// Réceptionner un message : déchiffrer le corps.
        /// </summary>
        /// <remarks>
        /// Cette fonction est destinée à être utilisée par un membre destinataire d'un message.
        /// </remarks>
        /// <param name="to">Certificat du destinataire.</param>
        /// <param name="messageSceal">Sceau du message.</param>
        /// <param name="inBodyStream">Flux d'entrée vers le corps du message chiffré à réceptionner.</param>
        /// <param name="outBodyStream">Flux de sortie vers le corps du message déchiffré.</param>
        /// <param name="cancellationToken">Jeton d'annulation.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task ReceiveMessageAsync(X509Certificate2 to, MessageSceal messageSceal, Stream inBodyStream, Stream outBodyStream, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(to);
            ArgumentNullException.ThrowIfNull(messageSceal);
            ArgumentNullException.ThrowIfNull(inBodyStream);
            ArgumentNullException.ThrowIfNull(outBodyStream);
            
            if (!inBodyStream.CanRead) throw new InvalidOperationException($"Input body stream is not readable.");
            if (!outBodyStream.CanWrite) throw new InvalidOperationException($"Output body stream is not writable.");
            CheckCertificate(to, mustHavePrivateKey: true);
            return ReceiveMessageInternalAsync(to, messageSceal, inBodyStream, outBodyStream, cancellationToken);
        }

        private async Task ReceiveMessageInternalAsync(X509Certificate2 to, MessageSceal messageSceal, Stream inBodyStream, Stream outBodyStream, CancellationToken cancellationToken)
        {
            //Récupérer la clef privée du destinataire
            var toPrivateKey = to.GetRSAPrivateKey() ?? throw new InvalidOperationException($"Certificate {to.Subject} ({to.SerialNumber}) does not have an RSA private key.");

            //déchiffrer les composants de la clef de chiffrement symétrique du message.
            var key = asymmetricCipherManager.DecryptData(toPrivateKey, messageSceal.EncryptedKey);
            var IV = asymmetricCipherManager.DecryptData(toPrivateKey, messageSceal.EncryptedIv);

            //instancier la clef de déchiffrement à partir de ses composants
            var sessionKey = sessionKeyManager.Create(key, IV);

            //Déchiffrer le contenu du corps du message
            await symmetricCipherManager.DecryptStreamAsync(sessionKey, inBodyStream, outBodyStream, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Vérifier un message.
        /// </summary>
        /// <remarks>
        /// Cette fonction est destinée à être utilisée par le service central NxC et par le membre destinataire d'un message pour en vérifier l'authentificité.
        /// </remarks>
        /// <param name="from">Certificat de l'expéditeur.</param>
        /// <param name="messageSceal">Données du sceau accompagnant le corps du message.</param>
        /// <param name="inBodyStream">Flux d'entrée vers le corps du message à vérifier.</param>
        /// <param name="cancellationToken">Jeton d'annulation.</param>
        /// <returns>Résultat da la vérification du message.</returns>
        public Task<MessageVerificationResult> VerifyMessageAsync(X509Certificate2 from, MessageSceal messageSceal, Stream inBodyStream, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(inBodyStream);
            if (!inBodyStream.CanRead) throw new InvalidOperationException($"Input body stream is not readable.");
            CheckCertificate(from, mustHavePrivateKey: false);
            return VerifyMessageInternalAsync(from, messageSceal, inBodyStream, cancellationToken);
        }

        private async Task<MessageVerificationResult> VerifyMessageInternalAsync(X509Certificate2 from, MessageSceal messageSceal, Stream inBodyStream, CancellationToken cancellationToken)
        {
            //Récupérer la clef public RSA de l'expéditeur
            var fromPublicKey = from.PublicKey.GetRSAPublicKey() ?? throw new InvalidOperationException($"Certificate {from.Subject} ({from.SerialNumber}) does not have an RSA public key.");
            try
            {
                //Calculer le hash du corps du message
                var encryptedFileHash = await hashManager.HashStreamAsync(inBodyStream, cancellationToken).ConfigureAwait(false);

                //Vérifier le hash
                var ok = hashManager.VerifyHash(fromPublicKey, encryptedFileHash, messageSceal.SignedFileHash);
                if (ok)
                    return OKResult($"Message body verification succeedeed - certificate {from.Subject} ({from.SerialNumber})");
                else
                    //échec de vérification du hash
                    return ErrorResult(null, VerificationResultCode.FailedHashVerification, $"VerifyHash failed - certificate {from.Subject} ({from.SerialNumber})");
            }
            catch (Exception ex)
            {
                return ErrorResult(ex, VerificationResultCode.UnexpectedError, ex.Message);
            }
        }

        private static MessageVerificationResult OKResult(string message)
        {
            return new MessageVerificationResult { VerificationResultCode = VerificationResultCode.Ok, Message = message };
        }

        private static MessageVerificationResult ErrorResult(Exception? ex, VerificationResultCode verificationResultCode, string? message)
        {
            return new MessageVerificationResult { Exception = ex, Message = message, VerificationResultCode = verificationResultCode };
        }

        /// <summary>
        /// Préparer un message pour l'envoi : chiffrement du corps du message et création du "sceau".
        /// </summary>
        /// <remarks>
        /// Cette fonction est destinée à être utilisée par le membre à l'origine du message.
        /// </remarks>
        /// <param name="from">Certificat de l'expéditeur (doit disposer d'une clef privée).</param>
        /// <param name="to">Certificat du destinataire.</param>
        /// <param name="inBodyStream">Flux d'entrée vers le corps du message à chiffrer.</param>
        /// <param name="outBodyStream">Flux de sortie vers le corps du message chiffré.</param>
        /// <param name="cancellationToken">Jeton d'annulation.</param>
        /// <returns>Sceau du message qui devra accompagner le corps du message afin de permettre d'en vérifier l'authenticité.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<MessageSceal> PrepareMessageAsync(X509Certificate2 from
                                                     , X509Certificate2 to
                                                     , Stream inBodyStream
                                                     , Stream outBodyStream
                                                     , CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);
            ArgumentNullException.ThrowIfNull(inBodyStream);
            ArgumentNullException.ThrowIfNull(outBodyStream);
            if (!inBodyStream.CanRead) throw new InvalidOperationException($"Input body stream is not readable.");
            if (!outBodyStream.CanWrite) throw new InvalidOperationException($"Output body stream is not writable.");
            if (!outBodyStream.CanSeek) throw new InvalidOperationException($"Output body stream is not seekable.");
            CheckCertificate(from, mustHavePrivateKey: true);
            CheckCertificate(to, mustHavePrivateKey: false);
            return PrepareMessageInternalAsync(from, to, inBodyStream, outBodyStream, cancellationToken);
        }

        private async Task<MessageSceal> PrepareMessageInternalAsync(X509Certificate2 from, X509Certificate2 to, Stream inBodyStream, Stream outBodyStream, CancellationToken cancellationToken)
        {
            //Récupérer la clef public RSA du destinataire
            var toPublicKey = to.PublicKey.GetRSAPublicKey() ?? throw new InvalidOperationException($"Certificate {to.Subject} ({to.SerialNumber}) does not have an RSA public key.");
            var fromPrivateKey = from.GetRSAPrivateKey() ?? throw new InvalidOperationException($"Certificate {from.Subject} ({from.SerialNumber}) does not have an RSA private key.");
            //Créer une clef de session pour le chiffrement symétrique du message.
            var sessionKey = sessionKeyManager.NewSessionKey();

            //Chiffrer le flux d'entrée avec la clef de chiffrement
            await symmetricCipherManager.EncryptStreamAsync(sessionKey, inBodyStream, outBodyStream, cancellationToken).ConfigureAwait(false);
            //Seek au début du fichier de sortie
            outBodyStream.Seek(0, SeekOrigin.Begin);
            //Calcul du hash du fichier chiffré
            byte[] cryptedFileHash = await hashManager.HashStreamAsync(outBodyStream, cancellationToken).ConfigureAwait(false);
            //Chiffrer les composantes de la clef de session avec la clef publique du destinataire
            byte[] encryptedKey = asymmetricCipherManager.EncryptData(toPublicKey, sessionKey.Key);
            byte[] encryptedIv = asymmetricCipherManager.EncryptData(toPublicKey, sessionKey.IV);
            //Signer le hash du fichier chiffré avec la clef privée de l'expéditeur
            byte[] signedFileHash = hashManager.SignHash(fromPrivateKey, cryptedFileHash);

            return new MessageSceal { EncryptedIv = encryptedIv, SignedFileHash = signedFileHash, EncryptedKey = encryptedKey };
        }

        private static void CheckCertificate(X509Certificate2 cert, bool mustHavePrivateKey)
        {
            if (mustHavePrivateKey && !cert.HasPrivateKey)
                throw new InvalidOperationException($"Certificate {cert.Subject} ({cert.SerialNumber}) does not have a private key.");
        }
    }
}