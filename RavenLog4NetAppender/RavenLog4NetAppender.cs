using System.Collections.Generic;

using SharpRaven;
using SharpRaven.Data;

using log4net.Appender;
using log4net.Core;

namespace RavenLog4NetAppender
{
    public class RavenLog4NetAppender : AppenderSkeleton
    {
        private static RavenClient ravenClient;
        public string DSN { get; set; }
        public string Logger { get; set; }


        protected override void Append(LoggingEvent loggingEvent)
        {
            if (ravenClient == null)
            {
                ravenClient = new RavenClient(DSN);
                ravenClient.Logger = Logger;
            }

            if (loggingEvent.ExceptionObject != null)
            {
                ravenClient.CaptureException(loggingEvent.ExceptionObject);
            }
            else
            {
                var level = Translate(loggingEvent.Level);
                // TODO: Handle log4net messages without an exception.
                var data = loggingEvent.MessageObject as IList<string>;

                if (data != null)
                {
                    foreach (string s in data)
                    {
                        // Do something with each string
                        ravenClient.CaptureMessage(s, level);
                    }
                }
            }
        }


        internal static ErrorLevel Translate(Level level)
        {
            return ErrorLevel.debug;
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