﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Configurations
{
    public class RsaSignature : RsaBase
    {
        public string SignatureKey { set; get; }
    }
}
