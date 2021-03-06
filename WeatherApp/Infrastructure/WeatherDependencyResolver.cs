﻿using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WeatherApp.Domain.Abstract;
using WeatherApp.Domain.Concrete;
using WeatherApp.Domain.Entities;

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
            kernel.Bind<IWeatherService>().To<WeatherServiceOwm>()
                .WithConstructorArgument("apiUri", WebConfigurationManager.AppSettings["ApiUriOWM"])
                .WithConstructorArgument("apiKey", WebConfigurationManager.AppSettings["ApiKeyOWM"]);
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>()
                .WithConstructorArgument("connectionString", "name=WeatherOwmDbLocal");
        }
    }
}