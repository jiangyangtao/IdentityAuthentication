using IdentityAuthentication.Model.Configurations;
using Newtonsoft.Json;

namespace Test.IdentityAuthentication.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var json = "{\"AuthenticationConfiguration\":{\"TokenType\":\"JWT\",\"EnableTokenRefresh\":true,\"EnableGrpcConnection\":true},\"AccessTokenConfiguration\":{\"RefreshTime\":600,\"ExpirationTime\":3600,\"Issuer\":\"IdentityAuthentication\",\"Audience\":\"Client\"},\"RefreshTokenConfiguration\":{\"ExpirationTime\":30,\"Issuer\":\"Authentication\",\"Audience\":\"Identity-Client\"},\"RsaVerifySignatureConfiguration\":{\"PublicKey\":\"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArlREt0S8iWue9mVjwjbAELLztEh3z6vbLyGL2hdVadmrHE1v3+7xMUvLn2HY+wE2hJNfNv6Ai5SuEZWlVg94n0tEqKcBs3tNskqIcpU684mt4INgtLl/c04oEkhEzFfcjv6QVMulPLAKEy13RnlsnWwob+sjEjonH+HcLMBwB8XW9EyhwFuAySjpAr6HVQ8lMJVeV1L45W0cO+PxEaFvvAoOwERpssBV3KY3dMi2USW6t8WcuY0qcLw4wdv+qCP9P0pzMbF98aJQUkZoL80GWtq/6HyD994ZRD9o/d1C3RqhQPs3mMByEqiu2X7JDi92/GphKZ9uQYMHx7an8PggfQIDAQAB\",\"AlgorithmType\":\"RsaSha256\",\"RSAKeyType\":\"Pkcs8\"}}";
            var config = JsonConvert.DeserializeObject<IdentityAuthenticationConfiguration>(json);

            Console.WriteLine(config.AuthenticationConfiguration.TokenType);
        }
    }
}