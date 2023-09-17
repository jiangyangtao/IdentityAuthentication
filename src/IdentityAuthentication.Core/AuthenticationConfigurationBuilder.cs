using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationConfigurationBuilder : IAuthenticationConfigurationBuilder
    {
        public AuthenticationConfigurationBuilder()
        {
        }


        public AccessTokenConfiguration AccessToken { set; get; }

        public RefreshTokenConfiguration RefreshToken { set; get; }



        public GrantDefaultConfiguration GrantDefault { set; get; }

        public AuthenticationConfiguration Authentication { set; get; }



        public RsaSignatureConfiguration RsaSignature { set; get; }

        public SymmetricSignatureConfiguration SymmetricSignature { set; get; }



        public RsaEncryptionConfiguration RsaEncryption { set; get; }

        public AesEncryptionConfiguration AesEncryption { set; get; }
    }
}
