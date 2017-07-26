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
        private readonly static ILog log = null;
        static Log()
        {
            ILoggerRepository repository = LogManager.CreateRepository("DefaultRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            log = LogManager.GetLogger(repository.Name, "DefaultLogger");
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public static void Debug(object message, Exception exception = null)
        {
            log.Debug(message, exception);
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public static void Info(object message, Exception exception = null)
        {
            log.Info(message, exception);
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public static void Warn(object message, Exception exception = null)
        {
            log.Warn(message, exception);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public static void Error(object message, Exception exception = null)
        {
            log.Error(message, exception);
        }
    }
}
