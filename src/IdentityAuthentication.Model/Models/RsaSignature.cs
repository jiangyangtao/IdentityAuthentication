
namespace IdentityAuthentication.Model.Models
{
    public class RsaSignature : RsaBase
    {
        public string SignatureKey { set; get; }

        public bool IsPublic { set; get; }
    }
}
