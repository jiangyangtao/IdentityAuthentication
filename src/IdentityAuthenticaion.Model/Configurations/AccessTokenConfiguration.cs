using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthenticaion.Model.Configurations
{
    public class AccessTokenConfiguration
    {
        public long ExpirationTime { set; get; }

        public long RefreshTime { set; get; } = 0;

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }
}
