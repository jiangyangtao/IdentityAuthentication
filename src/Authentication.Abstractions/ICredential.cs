
namespace Authentication.Abstractions
{
    public interface ICredential
    {
        public string AuthenticationType { get; }

        public string AuthenticationSource { get; }
    }
}
