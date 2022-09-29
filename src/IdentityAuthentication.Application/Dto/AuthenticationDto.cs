namespace IdentityAuthentication.Application.Dto
{
    public class AuthenticationDto
    {
        public string access_token { set; get; }

        public long expires_in { set; get; }

        public string token_type { set; get; }
    }
}
