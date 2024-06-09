using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookBridge.Persistance.Reflections
{
    public static class ReflectionServices
    {
        public static void AddInjectServices(this IServiceCollection collection, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly), "Assembly cannot be null");
            }

            var types = assembly.GetTypes().Where(type =>
                type is { IsAbstract: false, IsGenericTypeDefinition: false, IsInterface: false } &&
                type.Name.Contains("Service", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces().ToList();
                if (interfaces.Count == 0)
                {
                    continue;
                }

                foreach (var iFace in interfaces)
                {
                    collection.AddScoped(iFace, type);
                }
            }
        }
    }

}
