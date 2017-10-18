using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Central.API.Controllers;
using System.Net.Http;
using HiWork.BLL.ViewModels;
using HiWork.BLL.Services;
using HiWork.Utils.Infrastructure.Contract;
using HiWork.DAL.Repositories;
using HiWork.Utils.Infrastructure;
using System.Web.Script.Serialization;
using HiWork.BLL.Models;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System;
using Central.API.Config;

namespace Central.API.Auth
{
    public class CentralApiOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity("otf");
            //var username = context.OwinContext.Get<string>("otf:username");

            var user = context.OwinContext.Get<UserModel>("otf:user");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Username));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "user"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                { "CurrentUserID", user.CurrentUserID.ToString()},
                { " SessionId ",  user.SessionId.ToString()  },
                { "IsSuperAdmin", user.IsSuperAdmin.ToString()  },
                { "IsActive", user.IsActive.ToString() },
                { "Role",    user.Role.Name_en.ToString() },
                { "UserType",  user.UserType.Name_en.ToString() },
                { "Username",  user.Username.ToString() },
                { "Branch",  user.Branch },
                { "Team",  user.Team },
                { "Route", RouteParamMatch[user.UserType.Name_en.ToString()] }
            });

            var ticket = new AuthenticationTicket(identity, props);

            context.Validated(ticket);
            return Task.FromResult(0);
        }
        
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                var config = AppConfiguration.Config;

                var username = context.Parameters["username"];
                var password = context.Parameters["password"];
                var applicationId = context.Parameters["ApplicationId"];
                var CurrentCulture = context.Parameters["CurrentCulture"];
                IUnitOfWork uWork = new UnitOfWork();
                IUserRepository repo = new UserRepository(uWork);
                IUserService _userService = new UserService(repo);

               UserViewModel userViewModel = new UserViewModel();

                userViewModel.UserName = username;
                userViewModel.Password = password;
                userViewModel.ApplicationId = int.Parse(applicationId);
                userViewModel.CurrentCulture = CurrentCulture;

                var user = _userService.GetUser(userViewModel);

                if (user != null)
                {
                    context.OwinContext.Set("otf:user", user);
                    //context.OwinContext.Set("otf:username", username);
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {config.CORSOriginator});
                    //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "http://localhost:8000"});
                    //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "http://163.47.35.165:8082"});
                    
                    context.OwinContext.Response.StatusCode = 200;                    
                    context.Validated();
                }
                else
                {
                    context.SetError("Invalid credentials");
                    context.Rejected();
                }

            }
            catch (Exception ex)
            {
                context.SetError("Server error");
                context.Rejected();
            }
            return Task.FromResult(0);
        }

        public Dictionary<string, string> RouteParamMatch = new Dictionary<string, string>
        {
            { "SuperAdmin", "dashboard" },
            { "HR", "hrdashboard"},
            { "Accountant", "accountsdashboard" },
            { "Translator", "translatordashboard"},

            { "Employee", "dashboard" },
            { "Maintenance", "dashboard"},
            { "Guest", "dashboard" }
        };
    }
}