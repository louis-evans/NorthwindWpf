using Ninject;
using NorthwindWpf.Core.Configuration;
using System;

namespace NorthwindWpf.Core.Service
{
    public interface IServiceResolver : IDisposable
    {
        object Resolve(Type type);
        T Resolve<T>();
    }

    /// <summary>
    /// Singleton that acts as wrapper to resovle services using Ninject.
    /// Call <see cref="Get"/> to retrieve the single instance of <see cref="ServiceResolver"/>
    /// </summary>
    public class ServiceResolver : IServiceResolver
    {
        private static ServiceResolver _resolver;
        private readonly IKernel _kernal;

        private ServiceResolver()
        {
            _kernal = NinjectConfiguration.InitKernal();
        }

        /// <summary>
        /// Return the single instance of <see cref="ServiceResolver"/>
        /// </summary>
        /// <returns></returns>
        public static IServiceResolver Get()
        {
            if (_resolver == null)
            {
                _resolver = new ServiceResolver();
            }

            return _resolver;
        }

        public void Dispose() => _kernal.Dispose();

        public T Resolve<T>() => _kernal.Get<T>();

        public object Resolve(Type type) => _kernal.Get(type);
    }
}
