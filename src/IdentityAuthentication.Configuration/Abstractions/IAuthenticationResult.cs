namespace IdentityAuthentication.Configuration.Abstractions
{
    public interface IAuthenticationResult
    {
        static string UserIdPropertyName => nameof(UserId);

        static string UsernamePropertyName => nameof(Username);

        static string GrantSourcePropertyName => nameof(GrantSource);

        static string GrantTypePropertyName => nameof(GrantType);

        static string ClientPropertyName => nameof(Client);


        string UserId { get; }

        string Username { get; }

        string GrantSource { get; }

        string GrantType { get; }

        string Client { get; }

        IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
