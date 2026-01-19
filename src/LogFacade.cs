using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServerBackupper
{
    public static class LogFacade
    {
        private static ILogger logger;

        public static void Initialize(ILogger logFacade)
        {
            logger = logFacade;
        }

        #region LevelLogMethods
        public static void LogDebug(string? message, params object?[] args)
        {
            logger.LogDebug(message, args);
        }

        public static void LogTrace(string? message, params object?[] args)
        {
            logger.LogTrace(message, args);
        }

        public static void LogInformation(string? message, params object?[] args)
        {
            logger.LogInformation(message, args);
        }

        public static void LogWarning(string? message, params object?[] args)
        {
            logger.LogWarning(message: message, args: args);
        }

        public static void LogError(string? message, params object?[] args)
        {
            logger.LogError(message, args);
        }

        public static void LogCritical(string? message, params object?[] args)
        {
            logger.LogCritical(message, args);
        }
        #endregion
    }
}
