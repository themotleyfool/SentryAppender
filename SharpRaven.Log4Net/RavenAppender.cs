using System;
using System.Collections.Generic;

using SharpRaven.Data;

using log4net.Appender;
using log4net.Core;

namespace SharpRaven.Log4Net
{
    public class RavenAppender : AppenderSkeleton
    {
        private static RavenClient ravenClient;
        public string DSN { get; set; }
        public string Logger { get; set; }


        protected override void Append(LoggingEvent loggingEvent)
        {
            if (ravenClient == null)
            {
                ravenClient = new RavenClient(this.DSN);
                ravenClient.Logger = this.Logger;
            }

            if (loggingEvent.ExceptionObject != null)
            {
                ravenClient.CaptureException(loggingEvent.ExceptionObject);
            }
            else
            {
                var level = Translate(loggingEvent.Level);
                var stringList = loggingEvent.MessageObject as IList<string>;

                if (stringList != null)
                {
                    foreach (string s in stringList)
                    {
                        ravenClient.CaptureMessage(s, level);
                    }
                }

                var message = loggingEvent.MessageObject as string;

                if (message != null)
                {
                    ravenClient.CaptureMessage(message, level);
                }
            }
        }


        internal static ErrorLevel Translate(Level level)
        {
            switch (level.DisplayName)
            {
                case "WARN":
                    return ErrorLevel.warning;

                case "NOTICE":
                    return ErrorLevel.info;
            }

            ErrorLevel errorLevel;

            return !Enum.TryParse(level.DisplayName, true, out errorLevel)
                ? ErrorLevel.error
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