namespace IdentityAuthentication.Abstractions
{
    public interface IAuthenticationResult
    {
        string UserId { get; }

        string Username { get; }

        string GrantSource { get; }

        string GrantType { get; }

        string Client { get; }

        IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
