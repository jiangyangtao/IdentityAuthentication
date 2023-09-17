
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration.Abstractions
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
