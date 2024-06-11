using xDBAnalyticsExtractor.Utils;
using xDBAnalyticsExtractor.Exceptions;

namespace xDBAnalyticsExtractor.UnitTests.Utils
{
    [TestFixture]
    public class ExceptionContextBuilderTests
    {
        [Test]
        public void Build_GivenExceptionContext_SetsCorrectExceptionType()
        {
            // Arrange
            Type exceptionType = typeof(Exception);
            ExceptionContextBuilder builder = new ExceptionContextBuilder(exceptionType);

            // Act
            ExceptionContext context = builder.Build();

            // Assert
            Assert.That(context.ExceptionType, Is.EqualTo(exceptionType));
        }

        [Test]
        public void Build_GivenInteractionId_SetsInteractionId()
        {
            // Arrange
            Guid interactionId = Guid.NewGuid();
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithInteractionId(interactionId).Build();

            // Assert
            Assert.That(context.InteractionId, Is.EqualTo(interactionId));
        }

        [Test]
        public void Build_GivenEmptyInteractionId_SetsInteractionIdToDefaultGuid()
        {
            // Arrange
            Guid interactionId = Guid.Empty;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithInteractionId(interactionId).Build();

            // Assert
            Assert.That(context.InteractionId, Is.EqualTo(interactionId));
        }

        [Test]
        public void Build_GivenExceptionMessage_SetsExceptionMessage()
        {
            // Arrange
            string message = "Exception message";
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithExceptionMessage(message).Build();

            // Assert
            Assert.That(context.ExceptionMessage, Is.EqualTo(message));
        }

        [Test]
        public void Build_GivenNullExceptionMessage_SetsNullExceptionMessage()
        {
            // Arrange
            string message = null;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithExceptionMessage(message).Build();

            // Assert
            Assert.That(context.ExceptionMessage, Is.EqualTo(null));
        }

        [Test]
        public void Build_GivenExceptionStackTrace_SetsExceptionStackTrace()
        {
            // Arrange
            string stackTrace = "Stack trace";
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithExceptionStackTrace(stackTrace).Build();

            // Assert
            Assert.That(context.ExceptionStackTrace, Is.EqualTo(stackTrace));
        }

        [Test]
        public void Build_GivenNullExceptionStackTrace_SetsNullExceptionStackTrace()
        {
            // Arrange
            string stackTrace = null;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithExceptionStackTrace(stackTrace).Build();

            // Assert
            Assert.That(context.ExceptionStackTrace, Is.EqualTo(null));
        }

        [Test]
        public void Build_GivenTimestamp_SetsTimestamp()
        {
            // Arrange
            DateTime timestamp = DateTime.Now;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithTimestamp(timestamp).Build();

            // Assert
            Assert.That(context.Timestamp, Is.EqualTo(timestamp));
        }

        [Test]
        public void Build_GivenMaxTimestamp_SetsMaxTimestamp()
        {
            // Arrange
            DateTime timestamp = DateTime.MaxValue;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithTimestamp(timestamp).Build();

            // Assert
            Assert.That(context.Timestamp, Is.EqualTo(timestamp));
        }

        [Test]
        public void Build_GivenMinTimestamp_SetsMinTimestamp()
        {
            // Arrange
            DateTime timestamp = DateTime.MinValue;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithTimestamp(timestamp).Build();

            // Assert
            Assert.That(context.Timestamp, Is.EqualTo(timestamp));
        }

        [Test]
        public void Build_GivenCustomDataValue_AddsCustomDataWithRightValue()
        {
            // Arrange
            string key = "SomeKey";
            string value = "SomeValue";
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithCustomDataValue(key, value).Build();

            // Assert
            Assert.That(context.CustomData.ContainsKey(key), Is.True);
            Assert.That(context.CustomData[key], Is.EqualTo(value));
        }

        [Test]
        public void Build_GivenEmptyStringCustomDataKey_AddsCustomDataWithRightValue()
        {
            // Arrange
            string key = String.Empty;
            string value = "SomeValue";
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithCustomDataValue(key, value).Build();

            // Assert
            Assert.That(context.CustomData.ContainsKey(key), Is.True);
            Assert.That(context.CustomData[key], Is.EqualTo(value));
        }

        [Test]
        public void Build_GivenNullCustomDataValue_AddsCustomDataWithRightValue()
        {
            // Arrange
            string key = "SomeKey";
            string value = null;
            ExceptionContextBuilder builder = new ExceptionContextBuilder(typeof(Exception));

            // Act
            ExceptionContext context = builder.WithCustomDataValue(key, value).Build();

            // Assert
            Assert.That(context.CustomData.ContainsKey(key), Is.True);
            Assert.That(context.CustomData[key], Is.EqualTo(value));
        }

        [Test]
        public void Build_GivenAllParameters_FunctionsCorrectly()
        {
            // Arrange
            Type exceptionType = typeof(Exception);
            Guid interactionId = Guid.NewGuid();
            string message = "Exception message";
            string stackTrace = "Stack trace";
            DateTime timestamp = DateTime.Now;
            string key = "SomeKey";
            string value = "SomeValue";
            ExceptionContextBuilder builder = new ExceptionContextBuilder(exceptionType);

            // Act
            ExceptionContext context = builder
                .WithInteractionId(interactionId)
                .WithExceptionMessage(message)
                .WithExceptionStackTrace(stackTrace)
                .WithTimestamp(timestamp)
                .WithCustomDataValue(key, value)
                .Build();


            // Assert
            Assert.That(context.ExceptionType, Is.EqualTo(exceptionType));
            Assert.That(context.InteractionId, Is.EqualTo(interactionId));
            Assert.That(context.ExceptionMessage, Is.EqualTo(message));
            Assert.That(context.ExceptionStackTrace, Is.EqualTo(stackTrace));
            Assert.That(context.Timestamp, Is.EqualTo(timestamp));
            Assert.That(context.CustomData.ContainsKey(key), Is.True);
            Assert.That(context.CustomData[key], Is.EqualTo(value));
        }
    }
}
