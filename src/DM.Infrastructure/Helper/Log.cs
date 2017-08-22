using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DM.Infrastructure.Helper
{
    /// <summary>
    /// Log Helper
    /// </summary>
    public static class Log
    {
        private static ILog log = null;
        static Log()
        {
            ILoggerRepository repository = LogManager.CreateRepository("DefaultRepository");
            XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(AppContext.BaseDirectory + Path.DirectorySeparatorChar + "log4net.config"));
            log = LogManager.GetLogger(repository.Name, "DefaultLogger");
        }

        public static void Debug(object message, Exception exception = null)
        {
            log.Debug(message, exception);
        }

        public static void Info(object message, Exception exception = null)
        {
            log.Info(message, exception);
        }

        public static void Warn(object message, Exception exception = null)
        {
            log.Warn(message, exception);
        }

        public static void Error(object message, Exception exception = null)
        {
            log.Error(message, exception);
        }
    }
}
