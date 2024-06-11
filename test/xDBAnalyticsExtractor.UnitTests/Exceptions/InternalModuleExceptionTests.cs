using System.Text;
using xDBAnalyticsExtractor.Exceptions;


namespace xDBAnalyticsExtractor.UnitTests.Exceptions
{
    [TestFixture]
    public class InternalModuleExceptionTests
    {
        private ExceptionContext _validContextAllProperties;
        private ExceptionContext _invalidContextAllProperties;
        private ExceptionContext _validContextNoCustomData;
        private ExceptionContext _validContextCustomDataWithNullValue;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _validContextAllProperties = new ExceptionContext(typeof(InternalModuleException))
            {
                InteractionId = Guid.NewGuid(),
                ExceptionMessage = "Exception Message ...",
                ExceptionStackTrace = "Exception Stack Trace...",
                Timestamp = DateTime.Now,
                CustomData = new Dictionary<string, string?>
                {
                    { "CustomData Key A", "CustomData Value A" },
                    { "CustomData Key B", "CustomData Value B" }
                }
            };
            _invalidContextAllProperties = new ExceptionContext(typeof(InternalModuleException))
            {
                InteractionId = Guid.Empty,
                ExceptionMessage = null,
                ExceptionStackTrace = null,
                Timestamp = DateTime.Now,
                CustomData = null
            };
            _validContextNoCustomData = new ExceptionContext(typeof(InternalModuleException))
            {
                InteractionId = Guid.NewGuid(),
                ExceptionMessage = "Exception Message ...",
                ExceptionStackTrace = "Exception Stack Trace...",
                Timestamp = DateTime.Now
            };
            _validContextCustomDataWithNullValue = new ExceptionContext(typeof(InternalModuleException))
            {
                InteractionId = Guid.NewGuid(),
                ExceptionMessage = "Exception Message ...",
                ExceptionStackTrace = "Exception Stack Trace...",
                Timestamp = DateTime.Now,
                CustomData = new Dictionary<string, string?>
                {
                    { "Key", null },
                    { "OtherKey", "Value" }
                }
            };
        }

        [Test]
        public void InternalModuleExceptionConstructor_SetsExceptionContext()
        {
            // Arrange
            var context = _validContextAllProperties;

            // Act
            var exception = new TestInternalModuleException(context);

            // Assert
            Assert.That(exception.Context, Is.EqualTo(context) );
        }
        [Test]
        public void ToReadable_GivenValidExceptionInformation_ReturnsFormattedExceptionInformationCorrectly()
        {
            // Arrange
            var exception = new TestInternalModuleException(_validContextAllProperties);

            // Act
            string readableException = exception.ToReadable();

            // Assert
            StringBuilder expectedOutput = new();
            expectedOutput.AppendLine($"Exception Type: {typeof(InternalModuleException).FullName}");
            expectedOutput.AppendLine($"Interaction ID: {_validContextAllProperties.InteractionId}");
            expectedOutput.AppendLine($"Exception Message: {_validContextAllProperties.ExceptionMessage}");
            expectedOutput.AppendLine($"Exception Stack Trace: {_validContextAllProperties.ExceptionStackTrace}");
            expectedOutput.AppendLine($"Timestamp: {_validContextAllProperties.Timestamp}");
            expectedOutput.AppendLine("Custom Data:");
            expectedOutput.AppendLine("CustomData Key A: CustomData Value A");
            expectedOutput.AppendLine("CustomData Key B: CustomData Value B");

            Assert.That(readableException, Is.EqualTo(expectedOutput.ToString()));
        }
        [Test]
        public void ToReadable_GivenInvalidExceptionInformation_ReturnsFormattedExceptionInformationCorrectly()
        {
            // Arrange
            var exception = new TestInternalModuleException(_invalidContextAllProperties);

            // Act
            string readableException = exception.ToReadable();

            // Assert
            StringBuilder expectedOutput = new();
            expectedOutput.AppendLine($"Exception Type: {typeof(InternalModuleException).FullName}");
            expectedOutput.AppendLine($"Interaction ID: {_invalidContextAllProperties.InteractionId}");
            expectedOutput.AppendLine($"Exception Message: {_invalidContextAllProperties.ExceptionMessage}");
            expectedOutput.AppendLine($"Exception Stack Trace: {_invalidContextAllProperties.ExceptionStackTrace}");
            expectedOutput.AppendLine($"Timestamp: {_invalidContextAllProperties.Timestamp}");

            Assert.That(readableException, Is.EqualTo(expectedOutput.ToString()));
        }
        [Test]
        public void ToReadable_GivenNoCustomData_DoesNotContainCustomDataSection()
        {
            // Arrange
            var exception = new TestInternalModuleException(_validContextNoCustomData);

            // Act
            string readableException = exception.ToReadable();

            // Assert
            Assert.That(readableException.Contains("Custom Data:"), Is.False);
        }
        [Test]
        public void ToReadable_GivenNullCustomDataValues_ReturnsValuesCorrectly()
        {
            // Arrange
            var exception = new TestInternalModuleException(_validContextCustomDataWithNullValue);

            // Act
            string readableException = exception.ToReadable();

            // Assert
            StringBuilder expectedOutput = new();
            expectedOutput.AppendLine($"Exception Type: {typeof(InternalModuleException).FullName}");
            expectedOutput.AppendLine($"Interaction ID: {_validContextCustomDataWithNullValue.InteractionId}");
            expectedOutput.AppendLine($"Exception Message: {_validContextCustomDataWithNullValue.ExceptionMessage}");
            expectedOutput.AppendLine($"Exception Stack Trace: {_validContextCustomDataWithNullValue.ExceptionStackTrace}");
            expectedOutput.AppendLine($"Timestamp: {_validContextCustomDataWithNullValue.Timestamp}");
            expectedOutput.AppendLine("Custom Data:");
            expectedOutput.AppendLine("Key: ");
            expectedOutput.AppendLine("OtherKey: Value");

            Assert.That(readableException, Is.EqualTo(expectedOutput.ToString()));
        }

        private class TestInternalModuleException : InternalModuleException
        {
            public TestInternalModuleException(ExceptionContext context) : base(context)
            {
            }
        }
    }
    
}
