using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Abstractions.Credentials
{
    public class PasswordCredential : ICredential
    {
        public string AuthenticationType => "Password";

        public string Username { set; get; }

        public string Password { set; get; }

        public string AuthenticationSource { set; get; }
    }
}
