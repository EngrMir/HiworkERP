using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Central.API.Auth;

[assembly: OwinStartup(typeof(Central.API.Startup))]

namespace Central.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.UseOAuthAuthorizationServer(new CentralApiOAuthOptions());
            app.UseJwtBearerAuthentication(new CentralApiJwtOptions());
        }
    }
}
