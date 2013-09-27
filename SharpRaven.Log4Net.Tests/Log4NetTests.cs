using System;

using NUnit.Framework;

using log4net;

namespace SharpRaven.Log4Net.Tests
{
    [TestFixture]
    public class Log4NetTests
    {
        private ILog log;


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            this.log = LogManager.GetLogger(GetType());
        }


        [Test]
        [Explicit(
            "Only run this test if you're going to check for the logged error in Sentry or debug the SentryAppender.")]
        public void ErrorFormatWithMessage_MessageIsLogged()
        {
            this.log.ErrorFormat("This is a {0} message.", "test");
        }


        [Test]
        [Explicit(
            "Only run this test if you're going to check for the logged error in Sentry or debug the SentryAppender.")]
        public void ErrorWithException_ExceptionIsLogged()
        {
            this.log.Error("This is a test message", new Exception("This is a test exception"));
        }


        [Test]
        [Explicit(
            "Only run this test if you're going to check for the logged error in Sentry or debug the SentryAppender.")]
        public void ErrorWithMessage_MessageIsLogged()
        {
            this.log.Error("This is a test message.");
        }
    }
}