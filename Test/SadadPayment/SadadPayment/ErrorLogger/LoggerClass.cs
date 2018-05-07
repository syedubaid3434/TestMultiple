using System;
namespace ErrorLogger
{
    public sealed class LoggerClass
    {

        public delegate void LogEventHandler(object sender, LogEventArgs e);

        public LogEventHandler Log;

        //SingleTon 
        public static readonly  LoggerClass instance = new LoggerClass();

        private LoggerClass()
        {
             Severity = LogSeverity.Error;
        
        }

        public static LoggerClass Instance
        {
            get { return instance; }
        }

        private LogSeverity _severity;

        // These booleans are used strictly to improve performance.
        private bool _isDebug;
        private bool _isInfo;
        private bool _isWarning;
        private bool _isError;
        private bool _isFatal;

        /// <summary>
        /// Gets and sets the severity level of logging activity.
        /// </summary>
        public LogSeverity Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;

                // Set booleans to help improve performance
                int severity = (int)_severity;

                
                _isDebug = ((int)LogSeverity.Debug) >= severity ? true : false;
                _isInfo = ((int)LogSeverity.Info) >= severity ? true : false;
                _isWarning = ((int)LogSeverity.Warning) >= severity ? true : false;
                _isError = ((int)LogSeverity.Error) >= severity ? true : false;
                _isFatal = ((int)LogSeverity.Fatal) >= severity ? true : false;
            }
        }

        public void Debug(string message)
        { 
        if (_isDebug)
        {
            Debug(message,null);
        }

        }

        public void Debug(string message, string  location)
        {
            if(_isDebug)
                OnLog(new LogEventArgs(LogSeverity.Debug, message, location, DateTime.Now));
        }


        public void Info(string message)
        {
            if (_isInfo)
                Info(message, null);
        }

        public void Info(string message, string location)
        {
            if (_isInfo)
                OnLog(new LogEventArgs(LogSeverity.Info, message, location, DateTime.Now));
        }

        public void Error(string message)
        {
            if (_isError)
                Error(message, null);
        }

        public void Error(string message, string location)
        {
        if(_isError)
            OnLog(new LogEventArgs(LogSeverity.Error, message, location, DateTime.Now));
        }

        public void Fatal(string message)
        {
            if (_isFatal)
                Fatal(message, null);
        }

        public void Fatal(string message, string location)
        {
            if (_isFatal)
                OnLog(new LogEventArgs(LogSeverity.Fatal, message, location, DateTime.Now));
        }

        public void Warning(string message)
        {
            if (_isWarning)
                Warning(message, null);
        }

        public void Warning(string message, string location)
        {
            if (_isWarning)
                OnLog(new LogEventArgs(LogSeverity.Warning, message, location, DateTime.Now));
        }


        private void OnLog(LogEventArgs e)
        {
           if (Log != null)
            {
            Log(this,e);
            }
        }
        /// <summary>
        /// Attach a listening observer logging device to logger.
        /// </summary>
        /// <param name="observer">Observer (listening device).</param>
        public void Attach(ILog observer)
        {
            Log += observer.Log;
        }

        /// <summary>
        /// Detach a listening observer logging device from logger.
        /// </summary>
        /// <param name="observer">Observer (listening device).</param>
        public void Detach(ILog observer)
        {
            Log -= observer.Log;
        }

    }
}
