RavenLog4NetAppender
====================

log4net appender to send errors to Sentry (http://www.getsentry.com)

Configure in app.config:

```
<log4net>
	<root>
		<level value="DEBUG" />
		<appender-ref ref="RavenLog4NetAppender" />
	</root>
	<appender name="RavenLog4NetAppender" type="RavenLog4NetAppender.RavenLog4NetAppender, RavenLog4NetAppender">
		<DSN value="DSN_FROM_SENTRY_UI" />
		<Logger value="LOGGER_NAME" />
		<threshold value="ERROR" />
		<layout type="log4net.Layout.PatternLayout">
			<conversionPattern value="%5level - %message%newline" />
		</layout>
	</appender>
</log4net>
```
