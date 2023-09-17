namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenEncryptionProviderFactory
    {
        ITokenEncryptionProvider CreateTokenEncryptionProvider();
    }
}
