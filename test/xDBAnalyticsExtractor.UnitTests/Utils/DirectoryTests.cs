using System.Security.AccessControl;
using xDBAnalyticsExtractor.Utils;

namespace xDBAnalyticsExtractor.UnitTests.Utils;

[TestFixture]
public class DirectoryTests
{
    private string _tempPath;
    private string _writableDirectory;
    private string _nonWritableDirectory;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        // Get the system temp path
        _tempPath = Path.GetTempPath();

        // Create the writable directory
        _writableDirectory = Path.Combine(_tempPath, Path.GetRandomFileName());
        Directory.CreateDirectory(_writableDirectory);

        // Create the non writable directory
        _nonWritableDirectory = Path.Combine(_tempPath, Path.GetRandomFileName());
        Directory.CreateDirectory(_nonWritableDirectory);

        // Create a file in writable directory
        File.WriteAllText(Path.Combine(_writableDirectory, "test.txt"), "Hello!");
        
        // Create a file non-writableDirectory
        File.WriteAllText(Path.Combine(_nonWritableDirectory, "test.txt"), "World!");
        
        // This will work only for Windows machines, so adding pragma here...
#pragma warning disable CA1416
        // Set the non-writable directory's permissions to deny write
        DirectoryInfo di = new DirectoryInfo(_nonWritableDirectory);
        DirectorySecurity ds = di.GetAccessControl();


        ds.AddAccessRule(new FileSystemAccessRule(Environment.UserName,
            FileSystemRights.WriteData,
            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
            PropagationFlags.None,
            AccessControlType.Deny));
        
        di.SetAccessControl(ds);
#pragma warning restore CA1416
        

    }

    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void AssertDirectoryWritable_WhenProvidedWithInvalidDirPath_ThrowsException(string path)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.AssertDirectoryWritable(path));
    }

    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void AssertDirectoryWritable_WhenProvidedWithInvalidDirPath_HasCorrectErrorMessage(string path)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.AssertDirectoryWritable(path));
        StringAssert.Contains("The directory path is null, empty, or consists of whitespace only", ex.Message);
    }

    [Test]
    public void AssertDirectoryWritable_WhenProvidedWithInvalidDir_ThrowsException()
    {
        Assert.Throws<DirectoryNotFoundException>(
            () => IOUtils.AssertDirectoryWritable("this_directory_does_not_exist"));
    }
    
    [Test]
    public void AssertDirectoryWritable_WhenProvidedWithInvalidDir_HasCorrectErrorMessage()
    {
        const string nonExistentDirectory = "this_directory_does_not_exist";
        
        var ex = Assert.Throws<DirectoryNotFoundException>(
            () => IOUtils.AssertDirectoryWritable(nonExistentDirectory));
        
        StringAssert.Contains($"The directory {nonExistentDirectory} does not exist", ex.Message);
    }

    [Test]
    public void AssertDirectoryWritable_WhenProvidedWithAValidDir_DoesNotThrowException()
    {
        Assert.DoesNotThrow(()=>IOUtils.AssertDirectoryWritable(_writableDirectory));
    }

    [Test]
    public void AssertDirectoryWritable_WhenProvidedWithNonWritableDir_ThrowsUnauthorizedException()
    {
        Assert.Throws<UnauthorizedAccessException>(()=>IOUtils.AssertDirectoryWritable(_nonWritableDirectory));
    }

    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void AssertIsDirectory_WhenProvidedWithInvalidDirPath_ThrowsException(string path)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.AssertIsDirectory(path));
    }

    [TestCase("")]
    [TestCase("     ")]
    [TestCase(null)]
    public void AssertIsDirectory_WhenProvidedWithInvalidDirPath_HasCorrectErrorMessage(string path)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.AssertIsDirectory(path));
        StringAssert.Contains("The file path cannot be null or whitespace.", ex.Message);
    }

    [Test]
    public void AssertIsDirectory_WhenProvidedWithNonExistingDirectory_ThrowsException()
    {
        Assert.Throws<DirectoryNotFoundException>(
            () => IOUtils.AssertDirectoryWritable("this_directory_does_not_exist"));
    }

    [Test]
    public void AssertIsDirectory_WhenProvidedWithNonExistingDirectory_HasCorrectErrorMessage()
    {
        const string nonExistentDirectory = "this_directory_does_not_exist";
        
        var ex = Assert.Throws<DirectoryNotFoundException>(
            () => IOUtils.AssertDirectoryWritable(nonExistentDirectory));
        
        StringAssert.Contains($"The directory {nonExistentDirectory} does not exist", ex.Message);
    }
    
    [Test]
    public void AssertIsDirectory_WhenProvidedWithValidDirectory_DoesNotThrowException()
    {
        Assert.DoesNotThrow(()=>IOUtils.AssertIsDirectory(Path.GetTempPath()));
    }

    [Test]
    public void EnsureParentDirectoriesExist_WhenProvidedDirectoryDoesNotExist_CreatesDirectory()
    {
        var directoryPath = Path.Combine(_writableDirectory, Path.GetRandomFileName(), "test.txt");

        directoryPath = IOUtils.EnsureParentDirectoriesExist(directoryPath);
        
        Assert.That(Directory.Exists(directoryPath), Is.True);
    }

    [Test]
    public void EnsureParentDirectoriesExist_WhenProvidedDirectoryExists_DoesNothing()
    {
        var directoryPath = IOUtils.EnsureParentDirectoriesExist(_writableDirectory);
        
        Assert.That(Directory.Exists(directoryPath));
    }
    
    [Test]
    public void EnsureParentDirectoriesExist_WhenProvidedDirectoryHasInvalidCharacters_ThrowsIOException()
    {
        var directoryPath = Path.Combine(_writableDirectory, Path.GetRandomFileName(), "<>999%@$!!", "test.txt");

        Assert.Throws<IOException>(() => IOUtils.EnsureParentDirectoriesExist(directoryPath));
    }

    [TestCase("", "test.txt")]
    [TestCase("     ", "test.txt")]
    [TestCase(null, "test.txt")]
    public void IsDirectoryContaining_WhenProvidedWithInvalidDirPath_ThrowsException(string directory, string fileName)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.IsDirectoryContaining(directory, fileName));
    }

    [TestCase("", "test.txt")]
    [TestCase("     ", "test.txt")]
    [TestCase(null, "test.txt")]
    public void IsDirectoryContaining_WhenProvidedWithInvalidDirPath__HasCorrectErrorMessage(string directory, string filename)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.IsDirectoryContaining(directory, filename));
        StringAssert.Contains("The directory path is null, empty or consists of whitespace only", ex.Message);
    }
    
    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void IsDirectoryContaining_WhenProvidedWithInvalidFilename_ThrowsException(string fileName)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.IsDirectoryContaining(_writableDirectory, fileName));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void IsDirectoryContaining_WhenProvidedWithInvalidFilename__HasCorrectErrorMessage(string filename)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.IsDirectoryContaining(_writableDirectory, filename));
        StringAssert.Contains("The file name is null, empty, or consists of whitespace only.", ex.Message);
    }

    [TestCase]
    public void IsDirectoryContaining_WhenProvidedWithANonExistingDir_ReturnsFalse()
    {
        var directory = "this_directory_does_not_exist";
        Assert.That(IOUtils.IsDirectoryContaining(directory, "test.txt"), Is.False);
    }

    [Test]
    public void IsDirectoryContaining_WhenProvidedWithAValidWritableDirAndFile_ReturnsTrue()
    {
        Assert.That(IOUtils.IsDirectoryContaining(_writableDirectory, "test.txt"), Is.True);
    }
    
    [Test]
    public void IsDirectoryContaining_WhenProvidedWithAValidNonWritableDirAndFile_ReturnsTrue()
    {
        Assert.That(IOUtils.IsDirectoryContaining(_nonWritableDirectory, "test.txt"), Is.True);
    }
    
    [Test]
    public void IsDirectoryContaining_WhenProvidedWithAValidWritableDirAndNonExistingFile_ReturnsTrue()
    {
        Assert.That(IOUtils.IsDirectoryContaining(_writableDirectory, "test_not_exists.txt"), Is.False);
    }
    
    [Test]
    public void IsDirectoryContaining_WhenProvidedWithAValidNonWritableDirAndNonExistingFile_ReturnsTrue()
    {
        Assert.That(IOUtils.IsDirectoryContaining(_nonWritableDirectory, "test_not_exists.txt"), Is.False);
    }

    [Test]
    public void IsDirectoryContaining_WhenProvidedWithFilenameRelativePathComponents_IgnoresPathComponents()
    {
        Assert.That(IOUtils.IsDirectoryContaining(_writableDirectory, "..\\test.txt"));
    }

    [Test]
    public void IsDirectoryContaining_WhenProvidedWithFilenameHavingDirectorySeparators_IgnoresPathComponents()
    {
        Assert.That(IOUtils.IsDirectoryContaining(_writableDirectory, "\\test.txt"));
    }

    [Test]
    public void IsDirectory_WhenProvidedWithAValidDirectoryThatExists_ReturnsTrue()
    {
        Assert.That(IOUtils.IsDirectory(_writableDirectory), Is.True);
    }

    [Test]
    public void IsDirectory_WhenProvidedWithAValidDirectoryThatNotExists_ReturnsFalse()
    {
        Assert.That(IOUtils.IsDirectory("/this_directory_does_not_exist/"), Is.False);
    }

    [Test]
    public void IsDirectory_WhenProvidedDirectoryIsAFilePath_ReturnsFalse()
    {
        Assert.That(IOUtils.IsDirectory(Path.Combine(_writableDirectory, "test.txt")), Is.False);
    }
    
    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void IsDirectory_WhenProvidedWithInvalidPath_ThrowsException(string directory)
    {
        Assert.Throws<ArgumentException>(() => IOUtils.IsDirectory(directory));
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    public void IsDirectory_WhenProvidedWithInvalidPath_HasCorrectErrorMessage(string directory)
    {
        var ex = Assert.Throws<ArgumentException>(() => IOUtils.IsDirectory(directory));
        StringAssert.Contains("The path is null, empty, or contains only whitespace characters", ex.Message);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        // This will work only for Windows machines, so adding pragma here...
#pragma warning disable CA1416
        // Reset the non-writable directory's permissions to allow write
        DirectoryInfo di = new DirectoryInfo(_nonWritableDirectory);
        DirectorySecurity ds = di.GetAccessControl();
        ds.RemoveAccessRule(new FileSystemAccessRule(Environment.UserName,
            FileSystemRights.WriteData,
            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
            PropagationFlags.None,
            AccessControlType.Deny));
        di.SetAccessControl(ds);
#pragma warning disable CA1416

        // Delete the directories
        Directory.Delete(_writableDirectory, true);
        Directory.Delete(_nonWritableDirectory, true);
    }
}