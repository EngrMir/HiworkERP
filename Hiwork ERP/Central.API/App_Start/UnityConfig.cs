using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using HiWork.BLL.IOC;
using System.Web.Mvc;
using HiWork.BLL.Services;

namespace Central.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            DependencyInjector.Inject(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
           // DependencyResolver.SetResolver(new Unity.WebApi.UnityDependencyResolver(container));
        }
    }
}