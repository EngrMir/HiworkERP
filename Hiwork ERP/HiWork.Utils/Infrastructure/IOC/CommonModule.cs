using HiWork.Utils.Infrastructure.Contract;
using Ninject.Modules;

namespace HiWork.Utils.Infrastructure.IOC
{
    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IConnectionStringProvider>().To<ConnectionStringProvider>();
        }
    }
}
