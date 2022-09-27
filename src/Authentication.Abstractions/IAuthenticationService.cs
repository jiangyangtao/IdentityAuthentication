namespace Authentication.Abstractions
{
    public interface IAuthenticationService<in TCredential> where TCredential : ICredential
    {
        string AuthenticationSource { get; }

        Task<IAuthenticationResult> Authenticate(TCredential credential, CancellationToken cancellationToken);
    }
}