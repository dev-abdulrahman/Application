using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterServiceAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; }

        public RegisterServiceAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}
