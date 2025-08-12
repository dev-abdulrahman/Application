
using Application.Common;

namespace Application
{
    public static class AppSettings
    {
        private static IConfiguration? _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string? DatabaseConnectionString
        {
            get
            {
                return _configuration?.GetConnectionString(AppConstants.AppConnectionStringKey);
            }
        }
    }
}
