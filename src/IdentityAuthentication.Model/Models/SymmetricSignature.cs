
namespace IdentityAuthentication.Model.Models
{
    public abstract class SymmetricSignature
    {
        public string SignatureKey { set; get; }

        public abstract string Algorithm { get; }
    }
}
