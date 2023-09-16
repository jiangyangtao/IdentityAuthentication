using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Model
{
    public class Credentials
    {
        private readonly string SigningKey;
        private readonly string Algorithm;
        private readonly RSAKeyType? RSAKeyType;

        public Credentials(string signingKey, string algorithm)
        {
            if (signingKey.IsNullOrEmpty()) throw new ArgumentNullException(nameof(signingKey));
            if (algorithm.IsNullOrEmpty()) throw new ArgumentNullException(nameof(algorithm));

            SigningKey = signingKey;
            Algorithm = algorithm;
        }

        public Credentials(string signingKey, string algorithm, RSAKeyType keyType) : this(signingKey, algorithm)
        {
            RSAKeyType = keyType;
        }

        /// <summary>
        /// 签名的凭据
        /// </summary>
        /// <returns></returns>
        public SigningCredentials GenerateSigningCredentials()
        {
            var signingKey = GetSignatureSecurityKey();
            return new SigningCredentials(signingKey, Algorithm);
        }

        private SecurityKey GetSignatureSecurityKey()
        {
            if (RSAKeyType.HasValue) return GenerateRsaSecurityKey(RSAKeyType.Value);

            return GenerateSignatureSecurityKey();
        }

        public SecurityKey GenerateRsaSecurityKey(RSAKeyType keyType)
        {
            var rsa = RSA.Create();
            rsa.ImportPublicKey(keyType, SigningKey);

            return new RsaSecurityKey(rsa);
        }

        /// <summary>
        /// 生成签名的 <see cref="SecurityKey"/>
        /// </summary>
        /// <returns></returns>
        public SecurityKey GenerateSignatureSecurityKey()
        {
            var keyByteArray = Encoding.ASCII.GetBytes(SigningKey);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}
