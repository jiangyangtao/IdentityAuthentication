using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Models
{
    public abstract class SymmetricSignature
    {
        public string SignatureKey { set; get; }

        public string Algorithm { set; get; }
    }
}
