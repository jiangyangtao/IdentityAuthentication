namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenSignatureFactory
    {
        ITokenSignatureProvider CreateTokenSignatureProvider();
    }
}
