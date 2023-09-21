using IdentityAuthentication.Abstractions;

namespace Authentication.Abstractions
{
    public interface IAuthenticationService<in TCredential> where TCredential : ICredential
    {
        string GrantSource { get; }

        Task<IAuthenticationResult> AuthenticateAsync(TCredential credential);

        Task<bool> IdentityValidationAsync(string id, string username);
    }
}