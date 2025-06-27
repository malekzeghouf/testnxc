using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Certificate.Validation
{
    public interface ICertificateValidationManager
    {
        ValidationResult Validate(ValidationPolicy policy, X509Certificate2 certificate);
    }
}