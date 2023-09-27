namespace IdentityAuthentication.Model.Models
{
    public class TokenBaseConfiguration
    {
        public long ExpirationTime { set; get; }

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }
}
