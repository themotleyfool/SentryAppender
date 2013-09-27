using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;
using SharpRaven;

namespace RavenLog4NetAppender
{
	public class RavenLog4NetAppender : AppenderSkeleton
	{
		private static RavenClient ravenClient;

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
				// TODO: Handle log4net messages without an exception.
				var data = loggingEvent.MessageObject as IList<string>;

				if (data != null)
				{
					foreach (string s in data)
					{
						// Do something with each string
					}
				}
			}
		}

		protected override void Append(LoggingEvent[] loggingEvents)
		{
			foreach (var loggingEvent in loggingEvents)
			{
				Append(loggingEvent);
			}
		}

		public string DSN { get; set; }
		public string Logger { get; set; }
	} 
}