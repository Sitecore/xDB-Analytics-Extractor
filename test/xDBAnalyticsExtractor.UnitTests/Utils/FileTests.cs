using xDBAnalyticsExtractor.Utils;

namespace xDBAnalyticsExtractor.UnitTests.Utils;

[TestFixture]
public class FileTests
{
    private string _tempPath;
    private string _tempFile;
    private string _tempReadOnlyFile;

    private string _oldFileName;
    private string _newFileName;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // Get the system temp path and create a new directory
        _tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempPath);
        
        // Create a file in the directory
        _tempFile = Path.Combine(_tempPath, "test.txt");
        File.WriteAllText(_tempFile, "Hello!");
        
        // Create a read-only file in the directory
        _tempReadOnlyFile = Path.Combine(_tempPath, "readonly.txt");
        File.WriteAllText(_tempReadOnlyFile, "World!");
        
        // Set the file to read-only
        File.SetAttributes(_tempReadOnlyFile, FileAttributes.ReadOnly);

        _oldFileName = Path.GetTempFileName();
        _newFileName = Path.GetTempFileName();
        
        // Ensure the new file name does not exist yet.
        File.Delete(_newFileName);
    }

    [Test]
    public void AssertFileWritable_WhenProvidedWithAValidPath_DoesNotThrowException()
    {
        Assert.DoesNotThrow(()=> IOUtils.AssertFileWritable(_tempFile));
    }

    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void AssertFileWritable_WhenProvidedWithAnInvalidPath_ThrowsArgumentException(string filePath)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.AssertFileWritable(filePath));
    }
    
    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void AssertFileWritable_WhenProvidedWithAnInvalidPath_HasCorrectErrorMessage(string filePath)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.AssertFileWritable(filePath));
        StringAssert.Contains("The file path cannot be null or whitespace. (Parameter 'filePath')", ex.Message);
    }

    [Test]
    public void AssertFileWritable_WhenProvidedWithANonExistingFile_ThrowsFileNotFoundException()
    {
        const string filePath = "test-does-not-exist.txt";
        Assert.Throws<FileNotFoundException>(() => IOUtils.AssertFileWritable(filePath));
    }
    
    [Test]
    public void AssertFileWritable_WhenProvidedWithANonExistingFile_HasCorrectErrorMessage()
    {
        const string filePath = "test-does-not-exist.txt";
        var ex = Assert.Throws<FileNotFoundException>(() => IOUtils.AssertFileWritable(filePath));
        StringAssert.Contains($"The file {filePath} does not exist.", ex.Message,
            "The exception message should contain the file path {0}.", filePath);
    }

    [Test]
    public void AssertFileWritable_WhenProvidedWithAReadOnlyFile_ThrowsUnauthorizedAccessException()
    {
        Assert.Throws<UnauthorizedAccessException>(() => IOUtils.AssertFileWritable(_tempReadOnlyFile));
    }

    [Test]
    public void AssertIsFile_WhenProvidedWithAValidPath_DoesNotThrowException()
    {
        Assert.DoesNotThrow(()=> IOUtils.AssertIsFile(_tempFile));
    }
    
    [Test]
    public void AssertIsFile_WhenProvidedWithANonExistingPath_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => IOUtils.AssertIsFile("this-file-does-not-exist"));
    }

    [Test]
    public void AssertIsFile_WhenProvidedWithANonExistingPath_HasCorrectMessage()
    {
        const string filePath = "test-does-not-exist.txt";
        var ex = Assert.Throws<FileNotFoundException>(() => IOUtils.AssertIsFile(filePath));
        StringAssert.Contains("The file does not exist", ex.Message);
    }

    [Test]
    public void RenameFile_WhenProvidedWithValidFiles_RenamesFile()
    {
        IOUtils.RenameFile(_oldFileName, _newFileName);
        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(_oldFileName), Is.False);
            Assert.That(File.Exists(_newFileName), Is.True);
        });
    }

    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void RenameFile_OldNameIsInvalid_ThrowsArgumentException(string oldFileName)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.RenameFile(oldFileName, _newFileName));
    }
    
    [TestCase("")]
    [TestCase(null)]
    public void RenameFile_OldNameIsInvalid_HasCorrectErrorMessage(string oldFileName)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.RenameFile(oldFileName, _newFileName));
        StringAssert.Contains("Old file name cannot be null or empty. (Parameter 'oldFileName')", ex.Message);
    }
    
    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void RenameFile_NewNameIsInvalid_ThrowsArgumentException(string newFileName)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.RenameFile(_oldFileName, newFileName));
    }
    
    [TestCase("")]
    [TestCase(null)]
    public void RenameFile_NewNameIsInvalid_HasCorrectErrorMessage(string newFileName)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.RenameFile(_oldFileName, newFileName));
        StringAssert.Contains("New file name cannot be null or empty. (Parameter 'newFileName')", ex.Message);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        var readonlyAttributes = File.GetAttributes(_tempReadOnlyFile);
        readonlyAttributes &= ~FileAttributes.ReadOnly;
        
        File.SetAttributes(_tempReadOnlyFile, readonlyAttributes);
        
        // Clean up any files we've created.
        if (File.Exists(_oldFileName)) 
            File.Delete(_oldFileName);
        
        if (File.Exists(_newFileName)) 
            File.Delete(_newFileName);
        
        // Delete the directories
        Directory.Delete(_tempPath, true);
    }
}