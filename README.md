SentryAppender
=============

**NOTE** 
This repository is no longer being maintained by The Motley Fool.  Please review the network graph under Insights for a list of reposititores that have forked this and may have more up-to-date code.  Thanks!

[log4net](http://logging.apache.org/log4net/) appender to send errors to [Sentry](http://www.getsentry.com/).

Configure in app.config:

```xml
<log4net>
	<root>
		<level value="DEBUG" />
		<appender-ref ref="RavenAppender" />
	</root>
	<appender name="RavenAppender" type="SharpRaven.Log4Net.SentryAppender, SharpRaven.Log4Net">
		<DSN value="DSN_FROM_SENTRY_UI" />
		<Logger value="LOGGER_NAME" />
		<threshold value="ERROR" />
		<layout type="log4net.Layout.PatternLayout">
			<conversionPattern value="%5level - %message%newline" />
		</layout>
	</appender>
</log4net>
```
