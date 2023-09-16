﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Configurations
{
    public class RefreshTokenConfiguration 
    {
        public long ExpirationTime { set; get; }

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }
}
