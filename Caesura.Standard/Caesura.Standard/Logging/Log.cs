
using System;

namespace Caesura.Standard.Logging
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;
    
    public static class Log
    {
        public static ILoggingHandler Handler { get; set; }
        
        static Log()
        {
            
        }
        
        private static void RawLog(LogEventKind kind, LogSource source, Exception exception, String message, Object[] items)
        {
            if (Handler is null)
            {
                return;
            }
            
            var li = new LogInformation
            {
                Kind        = kind,
                Source      = source,
                Timestamp   = new TimeSpan(DateTime.UtcNow.Ticks),
                Message     = message,
                Exception   = exception,
                Items       = items,
            };
            Handler.Enqueue(li);
        }
        
        public static void Information(LogSource source, String message)
        {
            RawLog(LogEventKind.Information, source, null, message, null);
        }
        
        public static void Information(LogSource source, Exception exception)
        {
            RawLog(LogEventKind.Information, source, exception, null, null);
        }
        
        public static void Information(LogSource source, Exception exception, String message)
        {
            RawLog(LogEventKind.Information, source, exception, message, null);
        }
        
        public static void Information(LogSource source, String message, params Object[] items)
        {
            RawLog(LogEventKind.Information, source, null, message, items);
        }
        
        public static void Information(LogSource source, Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Information, source, exception, message, items);
        }
        
        public static void Information(String message)
        {
            RawLog(LogEventKind.Information, null, null, message, null);
        }
        
        public static void Information(Exception exception)
        {
            RawLog(LogEventKind.Information, null, exception, null, null);
        }
        
        public static void Information(Exception exception, String message)
        {
            RawLog(LogEventKind.Information, null, exception, message, null);
        }
        
        public static void Information(String message, params Object[] items)
        {
            RawLog(LogEventKind.Information, null, null, message, items);
        }
        
        public static void Information(Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Information, null, exception, message, items);
        }
        
        public static void Warning(LogSource source, String message)
        {
            RawLog(LogEventKind.Warning, source, null, message, null);
        }
        
        public static void Warning(LogSource source, Exception exception)
        {
            RawLog(LogEventKind.Warning, source, exception, null, null);
        }
        
        public static void Warning(LogSource source, Exception exception, String message)
        {
            RawLog(LogEventKind.Warning, source, exception, message, null);
        }
        
        public static void Warning(LogSource source, String message, params Object[] items)
        {
            RawLog(LogEventKind.Warning, source, null, message, items);
        }
        
        public static void Warning(LogSource source, Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Warning, source, exception, message, items);
        }
        
        public static void Warning(String message)
        {
            RawLog(LogEventKind.Warning, null, null, message, null);
        }
        
        public static void Warning(Exception exception)
        {
            RawLog(LogEventKind.Warning, null, exception, null, null);
        }
        
        public static void Warning(Exception exception, String message)
        {
            RawLog(LogEventKind.Warning, null, exception, message, null);
        }
        
        public static void Warning(String message, params Object[] items)
        {
            RawLog(LogEventKind.Warning, null, null, message, items);
        }
        
        public static void Warning(Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Warning, null, exception, message, items);
        }
        
        public static void Error(LogSource source, String message)
        {
            RawLog(LogEventKind.Error, source, null, message, null);
        }
        
        public static void Error(LogSource source, Exception exception)
        {
            RawLog(LogEventKind.Error, source, exception, null, null);
        }
        
        public static void Error(LogSource source, Exception exception, String message)
        {
            RawLog(LogEventKind.Error, source, exception, message, null);
        }
        
        public static void Error(LogSource source, String message, params Object[] items)
        {
            RawLog(LogEventKind.Error, source, null, message, items);
        }
        
        public static void Error(LogSource source, Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Error, source, exception, message, items);
        }
        
        public static void Error(String message)
        {
            RawLog(LogEventKind.Error, null, null, message, null);
        }
        
        public static void Error(Exception exception)
        {
            RawLog(LogEventKind.Error, null, exception, null, null);
        }
        
        public static void Error(Exception exception, String message)
        {
            RawLog(LogEventKind.Error, null, exception, message, null);
        }
        
        public static void Error(String message, params Object[] items)
        {
            RawLog(LogEventKind.Error, null, null, message, items);
        }
        
        public static void Error(Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Error, null, exception, message, items);
        }
        
        public static void Critical(LogSource source, String message)
        {
            RawLog(LogEventKind.Critical, source, null, message, null);
        }
        
        public static void Critical(LogSource source, Exception exception)
        {
            RawLog(LogEventKind.Critical, source, exception, null, null);
        }
        
        public static void Critical(LogSource source, Exception exception, String message)
        {
            RawLog(LogEventKind.Critical, source, exception, message, null);
        }
        
        public static void Critical(LogSource source, String message, params Object[] items)
        {
            RawLog(LogEventKind.Critical, source, null, message, items);
        }
        
        public static void Critical(LogSource source, Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Critical, source, exception, message, items);
        }
        
        public static void Critical(String message)
        {
            RawLog(LogEventKind.Critical, null, null, message, null);
        }
        
        public static void Critical(Exception exception)
        {
            RawLog(LogEventKind.Critical, null, exception, null, null);
        }
        
        public static void Critical(Exception exception, String message)
        {
            RawLog(LogEventKind.Critical, null, exception, message, null);
        }
        
        public static void Critical(String message, params Object[] items)
        {
            RawLog(LogEventKind.Critical, null, null, message, items);
        }
        
        public static void Critical(Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Critical, null, exception, message, items);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(LogSource source, String message)
        {
            RawLog(LogEventKind.Debug, source, null, message, null);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(LogSource source, Exception exception)
        {
            RawLog(LogEventKind.Debug, source, exception, null, null);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(LogSource source, Exception exception, String message)
        {
            RawLog(LogEventKind.Debug, source, exception, message, null);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(LogSource source, String message, params Object[] items)
        {
            RawLog(LogEventKind.Debug, source, null, message, items);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(LogSource source, Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Debug, source, exception, message, items);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(String message)
        {
            RawLog(LogEventKind.Debug, null, null, message, null);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(Exception exception)
        {
            RawLog(LogEventKind.Debug, null, exception, null, null);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(Exception exception, String message)
        {
            RawLog(LogEventKind.Debug, null, exception, message, null);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(String message, params Object[] items)
        {
            RawLog(LogEventKind.Debug, null, null, message, items);
        }
        
        [Conditional("DEBUG")]
        public static void Debug(Exception exception, String message, params Object[] items)
        {
            RawLog(LogEventKind.Debug, null, exception, message, items);
        }
    }
}
