using Serilog;

namespace Application.Common.Logging
{
    public static class ApplicationLog
    {
        public static void Info(string message)
        {
            Log.Information($"{AppConstants.ApplicationLogKey} - {message}");
        }

        public static void Warn(string message)
        {
            Log.Warning($"{AppConstants.ApplicationLogKey} - {message}");
        }

        public static void Error(string message, Exception? ex = null)
        {
            if (ex != null)
                Log.Error($"{AppConstants.ApplicationLogKey} - {ex}", $"{AppConstants.ApplicationLogKey} - {message}");
            else
                Log.Error($"{AppConstants.ApplicationLogKey} - {message}");
        }

        public static void Debug(string message)
        {
            Log.Debug($"{AppConstants.ApplicationLogKey} - {message}");
        }
    }
}
