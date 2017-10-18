using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Central.API.Config;

namespace Central.API.Auth
{

    public class CentralApiOAuthOptions : OAuthAuthorizationServerOptions
    {
        public CentralApiOAuthOptions()
        {
            var config = AppConfiguration.Config;

            TokenEndpointPath = new PathString(config.TokenPath);
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(config.ExpirationMinutes);
            AccessTokenFormat = new CentralApiJwtWriterFormat(this);
            Provider = new CentralApiOAuthProvider();
#if DEBUG
            AllowInsecureHttp = true;
#endif
        }
    }
}