using System.Numerics;
using xDBAnalyticsExtractor.Utils;

namespace xDBAnalyticsExtractor.UnitTests.Utils;

[TestFixture]
public class IOUtilsTests
{
    [Test]
    public void FormatByteSizeBI_WithNegativeSize_ShouldThrowArgumentException()
    {
        BigInteger invalid = -1;
        Assert.Throws<ArgumentException>(() => IOUtils.FormatByteSize(invalid));
    }
    
    [TestCase]
    public void FormatByteSizeBI_WithNegativeSize_HasCorrectErrorMessage()
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.FormatByteSize(new BigInteger(-1)));
        StringAssert.Contains($"Size cannot be negative", ex.Message);
    }

    [TestCaseSource(nameof(FormatByteSizeBiTestCases))]
    public void FormatByteSizeBI_WithVariousSizes_ShouldReturnCorrectString(BigInteger size, string expectedString)
    {
        var actual = IOUtils.FormatByteSize(size);
        Assert.That(actual, Is.EqualTo(expectedString));
    }

    [TestCase]
    public void FormatByteSizeLong_WithNegativeSize_ShouldThrowArgumentException()
    {
        var invalid = -1L;
        Assert.Throws<ArgumentException>(() => IOUtils.FormatByteSize(invalid));
    }

    [Test]
    public void FormatByteSizeLong_WithNegativeSize_HasCorrectErrorMessage()
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.FormatByteSize(-1));
        StringAssert.Contains($"Size cannot be negative", ex.Message);
    }

    [TestCase(0, "0 Bytes")]
    [TestCase(1023, "1023 Bytes")]
    [TestCase(1024, "1 KB")]
    [TestCase(1048576, "1 MB")]
    [TestCase(1073741824, "1 GB")]
    [TestCase(1099511627776, "1 TB")]
    [TestCase(1125899906842624, "1 PB")]
    public void FormatByreSizeLong_WithVariousSizes_ShouldReturnCorrectString(long size, string expectedString)
    {
        var actual = IOUtils.FormatByteSize(size);
        Assert.That(actual, Is.EqualTo(expectedString));
    }

    private static IEnumerable<TestCaseData> FormatByteSizeBiTestCases
    {
        get
        {
            yield return new TestCaseData(new BigInteger(0), "0 Bytes");
            yield return new TestCaseData(new BigInteger(1023), "1023 Bytes");
            yield return new TestCaseData(new BigInteger(1024), "1 KB");
            yield return new TestCaseData(new BigInteger(1048576), "1 MB");
            yield return new TestCaseData(new BigInteger(1073741824), "1 GB");
            yield return new TestCaseData(new BigInteger(1099511627776), "1 TB");
            yield return new TestCaseData(new BigInteger(1125899906842624), "1 PB");
        }
    }
}