using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AutoWire
{
    /// <summary>
    /// Extension methods for registration of classes using the <see cref="AutoServiceAttribute"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Scan all assemblies within the current <see cref="AppDomain"/> for any classes that have the <see cref="AutoServiceAttribute"/>
        /// applied and register them within the provided <paramref name="serviceCollection"/>
        /// </summary>
        /// <param name="serviceCollection">A service collection within which types should be registered</param>
        /// <returns>The <paramref name="serviceCollection"/> originally provided to this method</returns>
        public static IServiceCollection AutoWire(this IServiceCollection serviceCollection)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                AutoWire(serviceCollection, assembly);
            }

            return serviceCollection;
        }

        /// <summary>
        /// Scan all assemblies within the provided <see cref="Assembly"/> for any classes that have the <see cref="AutoServiceAttribute"/>
        /// applied and register them within the provided <paramref name="serviceCollection"/>
        /// </summary>
        /// <param name="serviceCollection">A service collection within which types should be registered</param>
        /// <param name="assembly">An assembly to search for classes with the <see cref="AutoServiceAttribute"/> applied</param>
        /// <returns>The <paramref name="serviceCollection"/> originally provided to this method</returns>
        public static IServiceCollection AutoWire(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var typesImplementingServiceAttribute = ScanForTypes(assembly);

            foreach (var tuple in typesImplementingServiceAttribute)
            {
                var type = tuple.Item1;
                var attribute = tuple.Item2;
                var serviceTypes = GetServiceTypes(type, attribute);
                
                foreach (var st in serviceTypes)
                {
                    var sd = new ServiceDescriptor(st, type, attribute.Lifetime);
                    serviceCollection.Add(sd);
                }
            }

            return serviceCollection;
        }

        private static Type[] GetServiceTypes(Type typeWithAttributeApplies, AutoServiceAttribute attribute)
        {
            if (attribute.ServiceTypes?.Length > 0)
            {
                return attribute.ServiceTypes;
            }

            var implementedInterfaces = typeWithAttributeApplies.GetInterfaces();
            if (implementedInterfaces?.Length > 0)
            {
                return implementedInterfaces;
            }

            return new[] {typeWithAttributeApplies};
        }

        private static IEnumerable<(Type, AutoServiceAttribute)> ScanForTypes(Assembly assembly)
        {
            return from type in assembly.GetTypes()
                where type.IsClass
                let attr = type.GetCustomAttribute<AutoServiceAttribute>()
                where attr != null
                select (type, attr);
        }
    }
}