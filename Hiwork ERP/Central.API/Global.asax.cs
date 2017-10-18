using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HiWork.BLL.ModelMapping;
using HiWork.Utils.Infrastructure;

namespace Central.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            AutoMapperBootStrapper.Initialize();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // JobScheduler.Start();
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            if (exc.GetType() == typeof(HttpException))
            {
                
            }
        }
    }
}
