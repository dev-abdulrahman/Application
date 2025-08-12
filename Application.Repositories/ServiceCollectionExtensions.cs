using Application.Repositories.Implementations;
using Application.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
