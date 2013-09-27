using NUnit.Framework;

using SharpRaven.Data;

using log4net.Core;

namespace SharpRaven.Log4Net.Tests
{
    [TestFixture]
    public class ErrorLevelTests
    {
        [Test]
        public void Translate_Debug_ReturnsDebug()
        {
            var level = RavenAppender.Translate(Level.Debug);
            Assert.That(level, Is.EqualTo(ErrorLevel.debug));
        }


        [Test]
        public void Translate_Error_ReturnsError()
        {
            var level = RavenAppender.Translate(Level.Error);
            Assert.That(level, Is.EqualTo(ErrorLevel.error));
        }


        [Test]
        public void Translate_Fatal_ReturnsFatal()
        {
            var level = RavenAppender.Translate(Level.Fatal);
            Assert.That(level, Is.EqualTo(ErrorLevel.fatal));
        }


        [Test]
        public void Translate_Info_ReturnsInfo()
        {
            var level = RavenAppender.Translate(Level.Info);
            Assert.That(level, Is.EqualTo(ErrorLevel.info));
        }


        [Test]
        public void Translate_Notice_ReturnsInfo()
        {
            var level = RavenAppender.Translate(Level.Notice);
            Assert.That(level, Is.EqualTo(ErrorLevel.info));
        }


        [Test]
        public void Translate_UnknownLevel_ReturnsError()
        {
            var level = RavenAppender.Translate(Level.Alert);
            Assert.That(level, Is.EqualTo(ErrorLevel.error));
        }


        [Test]
        public void Translate_Warn_ReturnsWarning()
        {
            var level = RavenAppender.Translate(Level.Warn);
            Assert.That(level, Is.EqualTo(ErrorLevel.warning));
        }
    }
}