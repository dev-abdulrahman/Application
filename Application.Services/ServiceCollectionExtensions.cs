using Application.Services.Attributes;
using Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServicesFromAssembly(this IServiceCollection services)
        {
            var applicationAssembly = typeof(BooksService).Assembly;

            if (applicationAssembly == null)
            {
                throw new ArgumentNullException(nameof(applicationAssembly), "Assembly cannot be null for attribute-based service registration.");
            }

            var typesToRegister = applicationAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition &&
                                                       t.GetCustomAttribute<RegisterServiceAttribute>() != null &&
                                                       t.Namespace != null &&
                                                       t.Namespace.StartsWith("Application.Services")
                                                       ).ToList();

            foreach (var type in typesToRegister)
            {
                var attribute = type.GetCustomAttribute<RegisterServiceAttribute>();
                var lifetime = attribute!.Lifetime;
                var interfaces = type.GetInterfaces();

                Type serviceType = null;

                // Option 1: Prefer an interface if it exists and follows a naming convention (e.g., I[ClassName])
                // Or just pick the first one if there's only one relevant
                if (interfaces.Any())
                {
                    // Attempt to find an interface that matches the class name (e.g., ProductService -> IProductService)
                    serviceType = interfaces.FirstOrDefault(i => i.Name == $"I{type.Name}");

                    // If no matching interface found, take the first one or throw an error if multiple are found and ambiguous
                    if (serviceType == null && interfaces.Length == 1)
                    {
                        serviceType = interfaces[0];
                    }
                    else if (serviceType == null && interfaces.Length > 1)
                    {
                        // Handle multiple interfaces: log a warning, throw an exception,
                        // or provide a mechanism to specify which interface the attribute refers to.
                        // For simplicity, we'll just fall back to registering as self if no clear interface.
                        // For more complex scenarios, you might add a property to ServiceAttribute (e.g., `RegisterAsType`)
                        Console.WriteLine($"Warning: Service '{type.Name}' implements multiple interfaces and no specific 'I{type.Name}' found. Registering as self.");
                        serviceType = type; // Fallback to registering as self
                    }
                }

                // If no suitable interface found or if it's a concrete class to be registered directly
                if (serviceType == null)
                {
                    serviceType = type; // Register the concrete type itself
                }

                switch (lifetime)
                {
                    case ServiceLifetime.Transient:
                        services.AddTransient(serviceType, type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(serviceType, type);
                        break;
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(serviceType, type);

                        break;
                }
            }
        }
    }
}
