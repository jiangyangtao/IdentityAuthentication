using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model.Configurations;
using Microsoft.Extensions.Options;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationConfigurationProvider : IAuthenticationConfigurationProvider
    {
        public AuthenticationConfigurationProvider(
            IOptions<GrantDefaultConfiguration> grantDefaultOption,
            IOptions<AuthenticationConfiguration> authenticationOption,
            IOptions<AccessTokenConfiguration> accessTokenOption,
            IOptions<RefreshTokenConfiguration> refreshTokenOption,
            IOptions<RsaSignatureConfiguration> rsaSignatureOption,
            IOptions<SymmetricSignatureConfiguration> symmetricSignatureOption,
            IOptions<RsaEncryptionConfiguration> rsaEncryptionOption,
            IOptions<AesEncryptionConfiguration> aesEncryptionOption
            )
        {
            GrantDefault = grantDefaultOption.Value;
            Authentication = authenticationOption.Value;
            AccessToken = accessTokenOption.Value;
            RefreshToken = refreshTokenOption.Value;

            if (rsaSignatureOption.Value != null) RsaSignature = rsaSignatureOption.Value;
            if (symmetricSignatureOption.Value != null) SymmetricSignature = symmetricSignatureOption.Value;
            if (rsaEncryptionOption.Value != null) RsaEncryption = rsaEncryptionOption.Value;
            if (aesEncryptionOption.Value != null) AesEncryption = aesEncryptionOption.Value; ;
        }


        public AccessTokenConfiguration AccessToken { get; }

        public RefreshTokenConfiguration RefreshToken { get; }



        public GrantDefaultConfiguration GrantDefault { get; }

        public AuthenticationConfiguration Authentication { get; }



        public RsaSignatureConfiguration? RsaSignature { get; }

        public SymmetricSignatureConfiguration? SymmetricSignature { get; }



        public RsaEncryptionConfiguration? RsaEncryption { get; }

        public AesEncryptionConfiguration? AesEncryption { get; }
    }
}
