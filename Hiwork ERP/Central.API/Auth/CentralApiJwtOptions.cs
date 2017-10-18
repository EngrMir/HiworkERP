using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.Jwt;


namespace Central.API.Auth
{
    public class CentralApiJwtOptions : JwtBearerAuthenticationOptions
    {
        public CentralApiJwtOptions()
        {
            var config = Config.AppConfiguration.Config;

            AllowedAudiences = new[] { config.JwtAudience };
            IssuerSecurityTokenProviders = new[]
            {
                new SymmetricKeyIssuerSecurityTokenProvider(config.JwtIssuer, config.JwtKey)
            };
        }
    }
}