using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenEncryptionProvider
    {
        TokenEncryptionType EncryptionType { get; }

        string Encrypt(string token);

        string Decrypt(string token);
    }
}
