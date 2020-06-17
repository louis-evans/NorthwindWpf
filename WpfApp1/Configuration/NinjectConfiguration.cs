using Ninject;
using Ninject.Modules;
using NorthwindWpf.Data.Repositories;
using NorthwindWpf.Data.Services;

namespace WpfApp1.Configuration
{
    public static class NinjectConfiguration
    {
        public static IKernel InitKernal()
        {
            return new StandardKernel(new ServiceModule(), new RepositoryModule());
        }
    }

    internal class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAddressLookupService>().To<AddressLookupService>();
        }
    }

    internal class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICustomerRepository>().To<CustomerRepository>();
            Bind<IOrderRepository>().To<OrderRepository>();
            Bind<IProductRepository>().To<ProductRepository>();
            Bind<IShipperRepository>().To<ShipperRepository>();
        }
    }
}
