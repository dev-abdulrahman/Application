using Application.Entities.Models;

namespace Application.Services.Interfaces
{
    public interface IApplicationLogsService
    {
        Task<IEnumerable<ApplicationLogs>> GetLogs();
        Task<ApplicationLogs> GetLogsById(int id);

    }
}
