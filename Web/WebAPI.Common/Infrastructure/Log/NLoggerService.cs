namespace WebAPI.Common.Infrastructure.Log
{
    public class NLoggerService : ILoggerService
    {
        private readonly NLog.ILogger _Logger;

        public NLoggerService(string name)
        {
            _Logger = NLog.LogManager.GetLogger(name);
        }

        public bool IsDebugEnabled()
        {
            return _Logger.IsDebugEnabled;
        }

        void ILoggerService.Debug(string message)
        {
            _Logger.Debug(message);
        }

        void ILoggerService.Error(string message)
        {
            _Logger.Error(message);
        }

        void ILoggerService.Error(string message, Exception exception)
        {
            _Logger.Error(exception, message);
        }

        void ILoggerService.Info(string message)
        {
            _Logger.Info(message);
        }

        void ILoggerService.Warn(string message)
        {
            _Logger.Warn(message);
        }
    }
}
