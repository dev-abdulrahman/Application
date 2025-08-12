using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Application.Entities.Models;
using Application.Services.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Implementations
{
    [RegisterService(ServiceLifetime.Scoped)]
    public class ApplicationLogsService : IApplicationLogsService
    {
        private readonly IGenericRepository<ApplicationLogs> _applicationLogsRepository;

        public ApplicationLogsService(IGenericRepository<ApplicationLogs> applicationLogsRepository)
        {
            _applicationLogsRepository = applicationLogsRepository;
        }

        public async Task<IEnumerable<ApplicationLogs>> GetLogs()
        {
            return await _applicationLogsRepository.GetAll();
        }

        public async Task<ApplicationLogs> GetLogsById(int id)
        {
            return await _applicationLogsRepository.GetFirstOrDefault(log => log.Id == id);
        }
    }
}
