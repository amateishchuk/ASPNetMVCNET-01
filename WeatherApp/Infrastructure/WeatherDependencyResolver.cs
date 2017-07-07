using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Services;

namespace WeatherApp.Infrastructure
{
    public class WeatherDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public WeatherDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IWeather>().To<ServiceOwm>();
        }
    }
}