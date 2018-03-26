using System.Linq;
using AutoWire.Tests.Support;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AutoWire.Tests
{
    public class ServiceCollectionTests
    {
        [Fact]
        public void CanRegisterTypeDecoratedWithDefaultAttribute()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AutoWire();

            var descriptors = from x in serviceCollection
                where x.ServiceType == typeof(WithDefaultAttributeService)
                      && x.Lifetime == ServiceLifetime.Singleton
                select x;

            Assert.Single(descriptors);
        }

        [Fact]
        public void CanRegisterTypeDecoratedWithSpecificLifetime()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AutoWire();

            var descriptors = from x in serviceCollection
                where x.ServiceType == typeof(WithScopedLifetimeService)
                      && x.Lifetime == ServiceLifetime.Scoped
                select x;

            Assert.Single(descriptors);
        }

        [Fact]
        public void CanRegisterTypeAgainstAllImplementingInterfaces()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AutoWire();

            var descriptors1 = from x in serviceCollection
                where x.ServiceType == typeof(IInterface1)
                      && x.ImplementationType == typeof(WithInterfaces)
                select x;

            var descriptors2 = from x in serviceCollection
                where x.ServiceType == typeof(IInterface2)
                      && x.ImplementationType == typeof(WithInterfaces)
                select x;

            Assert.Single(descriptors1);
            Assert.Single(descriptors2);
        }

        [Fact]
        public void CanRegisterTypeUsingSpecifiedInterfaces()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AutoWire();

            var descriptors1 = from x in serviceCollection
                where x.ServiceType == typeof(IInterface1)
                      && x.ImplementationType == typeof(WithSpecifiedInterfaces)
                select x;

            var descriptors2 = from x in serviceCollection
                where x.ServiceType == typeof(IInterface2)
                      && x.ImplementationType == typeof(WithSpecifiedInterfaces)
                select x;

            Assert.Empty(descriptors1);
            Assert.Single(descriptors2);
        }

        [Fact]
        public void CanRegisterTypeFromExternalAssemblyWhenScanningAllAssemblies()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AutoWire();

            var descriptors = from x in serviceCollection
                where x.ServiceType == typeof(ExternalType)
                      && x.ImplementationType == typeof(ExternalType)
                select x;

            Assert.Single(descriptors);
        }

        [Fact]
        public void CanRegisterTypeFromExternalAssemblyWhenSpecifyingAssembly()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AutoWire(typeof(ExternalType).Assembly);

            var descriptors = from x in serviceCollection
                where x.ServiceType == typeof(ExternalType)
                      && x.ImplementationType == typeof(ExternalType)
                select x;

            Assert.Single(descriptors);
            Assert.Single(serviceCollection);
        }

        [AutoService]
        private class WithDefaultAttributeService
        {
        }

        [AutoService(Lifetime = ServiceLifetime.Scoped)]
        private class WithScopedLifetimeService
        {
        }

        [AutoService]
        private class WithInterfaces : IInterface1, IInterface2
        {
        }

        [AutoService(typeof(IInterface2))]
        private class WithSpecifiedInterfaces : IInterface1, IInterface2
        {
        }

        private interface IInterface1
        {
        }

        private interface IInterface2
        {
        }
    }
}
