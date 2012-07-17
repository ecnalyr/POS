using System.Collections.Generic;
using System.Linq;
using Moq;
using POS.Domain.Concrete;
using POS.Domain.Entities;

[assembly: WebActivator.PreApplicationStartMethod(typeof(POS.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(POS.App_Start.NinjectWebCommon), "Stop")]

namespace POS.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Domain.Abstract;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            /*Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Categories).Returns(new List<Category>
                                                      {
                                                          new Category { CategoryId = 0, Name = "Balls" },
                                                          new Category { CategoryId = 1, Name = "Drinks" }
                                                      }.AsQueryable());
            mock.Setup(m => m.Products).Returns(new List<Product>
                                                    {
                                                        new Product { Name = "Football", Price = 25, Description = "Brown in color", CategoryId = 1,},
                                                        new Product { Name = "Soccerball", Price = 15 },
                                                        new Product { Name = "Volleyball", Price = 25 }
                                                    }.AsQueryable());
            kernel.Bind<IProductRepository>().ToConstant(mock.Object);*/
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
        }        
    }
}
