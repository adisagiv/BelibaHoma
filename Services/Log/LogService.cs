using log4net;

namespace Services.Log
{
    public static class LogService
    {
        private static readonly ILog _logger;

        public static ILog Logger {
            get { return _logger; } 
        }

        static LogService()
        {
            _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
