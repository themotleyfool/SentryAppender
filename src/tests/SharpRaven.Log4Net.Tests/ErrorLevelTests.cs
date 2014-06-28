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
            var level = SentryAppender.Translate(Level.Debug);
            Assert.That(level, Is.EqualTo(ErrorLevel.Debug));
        }


        [Test]
        public void Translate_Error_ReturnsError()
        {
            var level = SentryAppender.Translate(Level.Error);
            Assert.That(level, Is.EqualTo(ErrorLevel.Error));
        }


        [Test]
        public void Translate_Fatal_ReturnsFatal()
        {
            var level = SentryAppender.Translate(Level.Fatal);
            Assert.That(level, Is.EqualTo(ErrorLevel.Fatal));
        }


        [Test]
        public void Translate_Info_ReturnsInfo()
        {
            var level = SentryAppender.Translate(Level.Info);
            Assert.That(level, Is.EqualTo(ErrorLevel.Info));
        }


        [Test]
        public void Translate_Notice_ReturnsInfo()
        {
            var level = SentryAppender.Translate(Level.Notice);
            Assert.That(level, Is.EqualTo(ErrorLevel.Info));
        }


        [Test]
        public void Translate_UnknownLevel_ReturnsError()
        {
            var level = SentryAppender.Translate(Level.Alert);
            Assert.That(level, Is.EqualTo(ErrorLevel.Error));
        }


        [Test]
        public void Translate_Warn_ReturnsWarning()
        {
            var level = SentryAppender.Translate(Level.Warn);
            Assert.That(level, Is.EqualTo(ErrorLevel.Warning));
        }
    }
}