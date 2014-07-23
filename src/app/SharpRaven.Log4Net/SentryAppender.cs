using System;

using SharpRaven.Data;
using SharpRaven.Log4Net.Extra;

using log4net.Appender;
using log4net.Core;

namespace SharpRaven.Log4Net
{
    public class SentryAppender : AppenderSkeleton
    {
        private static RavenClient ravenClient;
        public string DSN { get; set; }
        public string Logger { get; set; }


        protected override void Append(LoggingEvent loggingEvent)
        {
            if (ravenClient == null)
            {
                ravenClient = new RavenClient(DSN)
                {
                    Logger = Logger
                };
            }

            object extra = new
            {
                Environment = new EnvironmentExtra(),
                Http = new HttpExtra(),
            };

            var exception = loggingEvent.ExceptionObject ?? loggingEvent.MessageObject as Exception;
            var level = Translate(loggingEvent.Level);

            if (exception != null)
            {
                ravenClient.CaptureException(exception, null, level, extra: extra);
            }
            else
            {
                var message = loggingEvent.RenderedMessage;

                if (message != null)
                {
                    ravenClient.CaptureMessage(message, level, null, extra);
                }
            }
        }


        internal static ErrorLevel Translate(Level level)
        {
            switch (level.DisplayName)
            {
                case "WARN":
                    return ErrorLevel.Warning;

                case "NOTICE":
                    return ErrorLevel.Info;
            }

            ErrorLevel errorLevel;

            return !Enum.TryParse(level.DisplayName, true, out errorLevel)
                       ? ErrorLevel.Error
                       : errorLevel;
        }


        protected override void Append(LoggingEvent[] loggingEvents)
        {
            foreach (var loggingEvent in loggingEvents)
            {
                Append(loggingEvent);
            }
        }
    }
}