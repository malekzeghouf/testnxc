using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Cryptography.Message
{
    public interface IMessageManager
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
        Task ReceiveMessageAsync(X509Certificate2 to, MessageSceal messageSceal, Stream inBodyStream, Stream outBodyStream, CancellationToken cancellationToken);

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
        Task<MessageSceal> PrepareMessageAsync(X509Certificate2 from
                                                     , X509Certificate2 to
                                                     , Stream inBodyStream
                                                     , Stream outBodyStream
                                                     , CancellationToken cancellationToken);

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
        /// <exception cref="InvalidOperationException"></exception>
        Task<MessageVerificationResult> VerifyMessageAsync(X509Certificate2 from, MessageSceal messageSceal, Stream inBodyStream, CancellationToken cancellationToken);
    }
}