using System;
using log4net;
using log4net.Core;
namespace Employee.Common
{
    public interface ILogger<T>
    {
        void LogError(string message, Exception exception);
    }
    public class Logger<T> :ILogger<T>
    {
        private readonly ILog _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger(typeof(T));
        }

        public void LogError(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }
    }
}
