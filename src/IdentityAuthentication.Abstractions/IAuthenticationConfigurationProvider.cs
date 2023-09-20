
using IdentityAuthentication.Configuration;
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Abstractions
{
    public interface IAuthenticationConfigurationProvider
    {
        AccessTokenConfiguration AccessToken { get; }

        RefreshTokenConfiguration RefreshToken { get; }



        GrantDefaultConfiguration GrantDefault { get; }

        AuthenticationConfiguration Authentication { get; }



        RsaSignatureConfiguration? RsaSignature { get; }

        SymmetricSignatureConfiguration? SymmetricSignature { get; }



        RsaEncryptionConfiguration? RsaEncryption { get; }

        AesEncryptionConfiguration? AesEncryption { get; }
    }
}
