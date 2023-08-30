using Microsoft.AspNetCore.Mvc;

namespace IdentityAuthentication.Application
{
    internal class AuthenticationConfigurationDefault
    {
        public static ApiVersion ApiV1 = new(1, 0);

        public const string DateFormatString = "yyyy-MM-dd HH:mm:ss";

        public const string GroupNameFormat = "'v'V";
    }
}
