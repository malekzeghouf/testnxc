using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Neoledge.NxC.Service.Certificate.Validation
{
    /// <summary>
    /// Validation des certificats
    /// </summary>
    /// <param name="logger"></param>
    public class CertificateValidationManager(ILogger<CertificateValidationManager> logger) : ICertificateValidationManager
    {
        public ValidationResult Validate(ValidationPolicy policy, X509Certificate2 certificate)
        {
            ArgumentNullException.ThrowIfNull(policy);
            ArgumentNullException.ThrowIfNull(certificate);
            if (policy.DisableAllValidation)
                return DisableAllValidationResult(policy, certificate);
            else
            {
                //Création de la chaîne de validation
                X509Chain validationChain = BuildChainFromPolicy(policy);
                //Validation de la chaîne
                bool validate = validationChain.Build(certificate);
                return ResultFromChain(policy, validate, validationChain);
            }
        }

        private ValidationResult DisableAllValidationResult(ValidationPolicy policy, X509Certificate2 certificate)
        {
            logger.LogWarning("Certificate {Subject}({SerialNumber}) validation with policy :{Identifier} .DisableAllValidation flag set in policy. Warning: this is a security risk and should not be used in a production environment."
                , certificate.Subject, certificate.SerialNumber, policy.PolicyIdentifier);
            return new ValidationResult { PolicyIdentifier = policy.PolicyIdentifier, Validated = true, Message = $"Warning : DisableAllValidation flag set in ValidationPolicy." };
        }

        private static ValidationResult ResultFromChain(ValidationPolicy policy, bool validate, X509Chain validationChain)
        {
            StringBuilder sbMessage = new();
            if (validationChain.ChainStatus.Length == 0)
                sbMessage.AppendLine("Chain status: N/A (no flags)");
            else
            {
                sbMessage.AppendLine("Chain status:");
                foreach (var status in validationChain.ChainStatus)
                    sbMessage.AppendLine($"- {status.Status}: {status.StatusInformation}");
                sbMessage.AppendLine();
            }
            sbMessage.AppendLine("Chain elements:");
            foreach (var certificate in validationChain.ChainElements.Select(c => c.Certificate))
                sbMessage.AppendLine($"- {certificate.Thumbprint} ({certificate.Subject}, expiry {certificate.GetExpirationDateString()})");
            sbMessage.AppendLine();
            return new ValidationResult { PolicyIdentifier = policy.PolicyIdentifier, Validated = validate, Message = sbMessage.ToString() };
        }

        private static X509Chain BuildChainFromPolicy(ValidationPolicy policy)
        {
            var chain = new X509Chain();

            //Contrôles de révocation
            chain.ChainPolicy.RevocationMode = policy.RevocationMode.Convert();
            chain.ChainPolicy.RevocationFlag = policy.RevocationFlag.Convert();
            //Contrôles de vérification
            chain.ChainPolicy.VerificationFlags = policy.VerificationFlags.Convert();
            //Racînes de confiances
            chain.ChainPolicy.TrustMode = policy.ChainTrustMode.Convert();
            foreach (var cert in policy.CustomTrustStore)
                chain.ChainPolicy.CustomTrustStore.Add(cert);
            //Stratégies d'utilisation
            policy.ApplicationPolicy.ConvertToOidCollection(chain.ChainPolicy.ApplicationPolicy);
            policy.CertificatePolicy.ConvertToOidCollection(chain.ChainPolicy.CertificatePolicy);
            //misc
            chain.ChainPolicy.DisableCertificateDownloads = policy.DisableCertificateDownloads;
            chain.ChainPolicy.VerificationTimeIgnored = policy.VerificationTimeIgnored;
            chain.ChainPolicy.VerificationTime = policy.VerificationTime == default(DateTime) ? DateTime.Now : policy.VerificationTime;

            return chain;
        }
    }
}