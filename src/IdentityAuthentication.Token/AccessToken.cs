using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Token
{
    internal class AccessToken
    {
        public string UserId { get; }

        public string Username { get; }

        public string GrantSource { get; }

        public string GrantType { get; }

        public string Client { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
