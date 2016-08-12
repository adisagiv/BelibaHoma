using log4net;

namespace Services.Log
{
    public class LogService : ILogService
    {
        private readonly ILog _logger;

        public ILog Logger {
            get { return _logger; } 
        }

        public LogService()
        {
            _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
