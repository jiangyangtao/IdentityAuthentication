
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Common
{
    public class AuthenticationConfig
    {
        public string EncryptionAlgorithm { set; get; } = SecurityAlgorithms.RsaSha256;
    }

    public class AccessTokenConfig
    {
        public long ExpirationTime { set; get; }

        public long RefreshTime { set; get; } = 0;

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }

    public class RefreshTokenConfig : AccessTokenConfig
    {

    }

    public class SecretKeyConfig
    {
        /// <summary>
        /// HmacSha256
        /// </summary>
        public string HmacSha256Key { set; get; }

        public string RsaPublicKey { set; get; }

        public string RsaPrivateKey { set; get; }
    }
}
