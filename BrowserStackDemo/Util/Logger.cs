namespace LLAutomation.Util
{
    using NLog;
    using System.Reflection;

    public static class Logger
    {
        private static NLog.Logger _logger; //NLog logger  
        // This will get the current WORKING directory (i.e. \bin\Debug)
        static string workingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);         // This will get the current PROJECT directory
        static string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName; 
        static string configFilePath = projectDirectory + Path.DirectorySeparatorChar + "nlog.config"; 
        static Logger()
        {
            LogManager.LoadConfiguration(configFilePath);
        }
        #region Public Methods  
        /// <summary>  
                /// This method writes the Debug information to trace file  
                /// </summary>  
                /// <param name="message">The message.</param>  
        public static void Debug(String pageName, String message)
        {
            _logger = LogManager.GetLogger(pageName);
            if (!_logger.IsDebugEnabled) return;
            _logger.Debug(message);
        }         /// <summary>  
                          /// This method writes the Information to trace file  
                          /// </summary>  
                          /// <param name="message">The message.</param>  
        public static void Info(String pageName, String message)
        {
            _logger = LogManager.GetLogger(pageName);
            if (!_logger.IsInfoEnabled) return;
            _logger.Info(message);
        }         /// <summary>  
                          /// This method writes the Warning information to trace file  
                          /// </summary>  
                          /// <param name="message">The message.</param>  
        public static void Warn(String message)
        {
            if (!_logger.IsWarnEnabled) return;
            _logger.Warn(message);
        }         /// <summary>  
                          /// This method writes the Error Information to trace file  
                          /// </summary>  
                          /// <param name="error">The error.</param>  
                          /// <param name="exception">The exception.</param>  
        public static void Error(String pageName, String error, Exception exception = null)
        {
            _logger = LogManager.GetLogger(pageName);
            if (!_logger.IsErrorEnabled) return;
            _logger.ErrorException(error, exception);
        }         /// <summary>  
                          /// This method writes the Fatal exception information to trace target  
                          /// </summary>  
                          /// <param name="message">The message.</param>  
        public static void Fatal(String message)
        {
            if (!_logger.IsFatalEnabled) return;
            _logger.Warn(message);
        }         /// <summary>  
                          /// This method writes the trace information to trace target  
                          /// </summary>  
                          /// <param name="message">The message.</param>  
        public static void Trace(String message)
        {
            if (!_logger.IsTraceEnabled) return;
            _logger.Trace(message);
        }

        public static void Info( String message)
        {
            _logger = LogManager.GetLogger("");
            if (!_logger.IsInfoEnabled) return;
            _logger.Info(message);
        }

        public static void Error( String error, Exception exception = null)
        {
            _logger = LogManager.GetLogger("");
            if (!_logger.IsErrorEnabled) return;
            _logger.ErrorException(error, exception);
        }
        #endregion
    }
}