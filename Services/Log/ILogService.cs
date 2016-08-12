using log4net;

namespace Services.Log
{
    public interface ILogService
    {
        ILog Logger { get; }
    }
}
