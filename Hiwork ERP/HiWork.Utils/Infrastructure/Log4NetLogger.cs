using System;
using System.Threading.Tasks;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace HiWork.Utils.Infrastructure
{
    public static class Log4NetLogger
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(Log4NetLogger));

        static Log4NetLogger()
        {
        }

        public static void Error(object msg)
        {
            Task.Run(() =>  Log.Error(msg));
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        public static void Info(object msg)
        {
            Task.Run(() =>  Log.Info(msg));
        }

        public static void Init()
        {
        }
    }
}
