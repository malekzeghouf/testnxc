using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Neoledge.NxC.Service.Certificate.Validation
{
    internal static class Converter
    {
        internal static X509RevocationMode Convert(this RevocationMode revocationMode)
        {
            return revocationMode switch
            {
                RevocationMode.NoCheck => X509RevocationMode.NoCheck,
                RevocationMode.Online => X509RevocationMode.Online,
                RevocationMode.Offline => X509RevocationMode.Offline,
                _ => throw new NotImplementedException($"{nameof(RevocationMode)} {revocationMode} not implemented"),
            };
        }

        internal static X509RevocationFlag Convert(this RevocationFlag revocationFlag)
        {
            return revocationFlag switch
            {
                RevocationFlag.EntireChain => X509RevocationFlag.EntireChain,
                RevocationFlag.ExcludeRoot => X509RevocationFlag.ExcludeRoot,
                RevocationFlag.EndCertificateOnly => X509RevocationFlag.EndCertificateOnly,
                _ => throw new NotImplementedException($"{nameof(RevocationFlag)} {revocationFlag} not implemented"),
            };
        }

        internal static X509VerificationFlags Convert(this VerificationFlags verificationFlags)
        {
            if(verificationFlags.HasFlag(VerificationFlags.None)) return X509VerificationFlags.NoFlag;
            if(verificationFlags.HasFlag(VerificationFlags.AllFlags)) return X509VerificationFlags.AllFlags;
            X509VerificationFlags r = new();
            if(verificationFlags.HasFlag(VerificationFlags.AllowUnknownCertificateAuthority)) r |= X509VerificationFlags.AllowUnknownCertificateAuthority;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown)) r |= X509VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreCtlNotTimeValid)) r |= X509VerificationFlags.IgnoreCtlNotTimeValid;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreCtlSignerRevocationUnknown)) r |= X509VerificationFlags.IgnoreCtlSignerRevocationUnknown;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreEndRevocationUnknown)) r |= X509VerificationFlags.IgnoreEndRevocationUnknown;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreInvalidBasicConstraints)) r |= X509VerificationFlags.IgnoreInvalidBasicConstraints;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreInvalidName)) r |= X509VerificationFlags.IgnoreInvalidName;
            if(verificationFlags.HasFlag(VerificationFlags.IgnoreInvalidPolicy)) r |= X509VerificationFlags.IgnoreInvalidPolicy;
            if (verificationFlags.HasFlag(VerificationFlags.IgnoreNotTimeNested)) r |= X509VerificationFlags.IgnoreNotTimeNested;
            if (verificationFlags.HasFlag(VerificationFlags.IgnoreNotTimeValid)) r |= X509VerificationFlags.IgnoreNotTimeValid;
            if (verificationFlags.HasFlag(VerificationFlags.IgnoreRootRevocationUnknown)) r |= X509VerificationFlags.IgnoreRootRevocationUnknown;
            if (verificationFlags.HasFlag(VerificationFlags.IgnoreWrongUsage)) r |= X509VerificationFlags.IgnoreWrongUsage;

            return r;
        }

        internal static X509ChainTrustMode Convert(this ChainTrustMode chainTrustMode)
        {
            return chainTrustMode switch
            {
                ChainTrustMode.System => X509ChainTrustMode.System,
                ChainTrustMode.CustomRootTrust => X509ChainTrustMode.CustomRootTrust,
                _ => throw new NotImplementedException($"{nameof(ChainTrustMode)} {chainTrustMode} not implemented"),
            };
        }

        internal static void ConvertToOidCollection(this IList<string> oidList, OidCollection result)
        {
            foreach (string oid in oidList)
                result.Add(new Oid(oid));
        }
    }
}
