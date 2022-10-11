
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Common
{
    public class AuthenticationConfig
    {
        public long TokenExpirationTime { set; get; }

        public long TokenRefreshTime { set; get; }

        public string AccessIssuer { set; get; }

        public string RefreshIssuer { set; get; }

        public string Audience { set; get; }

        public string EncryptionAlgorithm { set; get; } = SecurityAlgorithms.RsaSha256;
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
