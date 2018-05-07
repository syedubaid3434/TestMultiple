using System;

namespace ErrorLogger
{
    public class LogEventArgs :EventArgs
    {
        /// <summary>
        /// Constructor of LogEventArgs.
        /// </summary>
        /// <param name="severity">Log severity.</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Inner exception.</param>
        /// <param name="date">Log date.</param>
        public LogEventArgs(LogSeverity severity, string message, string  location, DateTime date)
        {
            Severity = severity;
            Message = message;
            Location = location;
            Date = date;
        }

        /// <summary>
        /// Gets and sets the log severity.
        /// </summary>        
        public LogSeverity Severity { get; private set; }

        /// <summary>
        /// Gets and sets the log message.
        /// </summary>        
        public string Message { get; private set; }

        /// <summary>
        /// Gets and sets the optional inner exception.
        /// </summary>        
        public string Location { get; private set; }

        /// <summary>
        /// Gets and sets the log date and time.
        /// </summary>        
        public DateTime Date { get; private set; }

        /// <summary>
        /// Friendly string that represents the severity.
        /// </summary>
        public String SeverityString
        {
            get { return Severity.ToString("G"); }
        }

        /// <summary>
        /// LogEventArgs as a string representation.
        /// </summary>
        /// <returns>String representation of the LogEventArgs.</returns>
        public override String ToString()
        {
            return "" + Date
                + " - " + SeverityString
                + " - " + Message
                + " - " + Location.ToString();
        }
    }
}
