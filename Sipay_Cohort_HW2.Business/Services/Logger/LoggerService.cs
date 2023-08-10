using Serilog;

namespace Sipay_Cohort_HW2.Business.Services.Logger
{
    public class LoggerService:ILoggerService
    {
        public void Write(string message)
        {
            Log.Error("[Logger] - " + message);

        }
    }
}
