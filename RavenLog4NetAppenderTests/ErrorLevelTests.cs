using NUnit.Framework;

using SharpRaven.Data;

using log4net.Core;


namespace RavenLog4NetAppenderTests
{
    [TestFixture]
    public class ErrorLevelTests
    {
        [Test]
        public void Translate_UnknownLevel_ReturnsError()
        {
            var level = RavenLog4NetAppender.RavenLog4NetAppender.Translate(Level.Alert);
            Assert.That(level, Is.EqualTo(ErrorLevel.error));
        }
    }
}