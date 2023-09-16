using IdentityAuthentication.Model.Models;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System.Diagnostics.CodeAnalysis;
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

        public Credentials([NotNull] SymmetricSignature symmetricSignature) : this(symmetricSignature.SignatureKey, symmetricSignature.Algorithm)
        {
        }

        public Credentials([NotNull] RsaSignature rsaSignature) : this(rsaSignature.SignatureKey, rsaSignature.RsaAlgorithm)
        {
            RSAKeyType = rsaSignature.RSAKeyType;
        }

        /// <summary>
        /// 生成签名的 <see cref="SigningCredentials"/>
        /// </summary>
        /// <returns></returns>
        public SigningCredentials GenerateSigningCredentials()
        {
            var signingKey = GenerateSignatureSecurityKey();
            return new SigningCredentials(signingKey, Algorithm);
        }

        /// <summary>
        /// 生成签名的 <see cref="SecurityKey"/>
        /// </summary>
        /// <returns></returns>
        public SecurityKey GenerateSignatureSecurityKey()
        {
            if (RSAKeyType.HasValue) return GenerateRsaSecurityKey(RSAKeyType.Value);

            return GenerateSymmetricSecurityKey();
        }

        private SecurityKey GenerateRsaSecurityKey(RSAKeyType keyType)
        {
            var rsa = RSA.Create();
            rsa.ImportPublicKey(keyType, SigningKey);

            return new RsaSecurityKey(rsa);
        }

        private SecurityKey GenerateSymmetricSecurityKey()
        {
            var keyByteArray = Encoding.ASCII.GetBytes(SigningKey);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}
