using System.Numerics;

namespace xDBAnalyticsExtractor.Utils;

/// <summary>
///     General file manipulation utilities.
/// </summary>
public static class IOUtils
{
    // The number of bytes in a kilobyte
    private const long ONE_KB = 1024;

    // The number of bytes in a megabyte
    private const long ONE_MB = ONE_KB * ONE_KB;

    // The number of bytes in a gigabyte
    private const long ONE_GB = ONE_KB * ONE_MB;

    // The number of bytes in a terabyte
    private const long ONE_TB = ONE_KB * ONE_GB;

    // The number of bytes in a petabyte
    private const long ONE_PB = ONE_KB * ONE_TB;

    private static readonly BigInteger ONE_KB_BI = new(ONE_KB);

    private static readonly BigInteger ONE_MB_BI = new(ONE_MB);

    private static readonly BigInteger ONE_GB_BI = new(ONE_GB);

    private static readonly BigInteger ONE_TB_BI = new(ONE_TB);

    private static readonly BigInteger ONE_PB_BI = new(ONE_PB);

    /// <summary>Checks if the specified directory has write access. If it does not, an IOUtilsException is thrown.</summary>
    /// <param name="directory">The directory to check for write access.</param>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="directory"/> is null, empty, or consists of whitespace only.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the <paramref name="directory"/> does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the <paramref name="directory"/> is not writable.</exception>
    /// <exception cref="IOException">Thrown if an error occurs while creating or deleting the test file.</exception>
    public static void AssertDirectoryWritable(string directory)
    {
        if (string.IsNullOrWhiteSpace(directory))
            throw new ArgumentException("The directory path is null, empty, or consists of whitespace only");

        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"The directory {directory} does not exist");

        var testFilePath = Path.Combine(directory, Path.GetRandomFileName());
        using var fs = File.Create(testFilePath);

        fs.Close();

        File.Delete(testFilePath);
    }

    /// <summary>Checks if the specified file is writable. If it is not, an IOUtilsException is thrown.</summary>
    /// <param name="filePath">The path of the file to check for write access.</param>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="filePath"/> is null, empty, or consists of whitespace only.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the <paramref name="filePath"/> does not exist.</exception>
    /// <exception cref="IOException">Thrown if an error occurs while opening the file for writing.</exception>
    public static void AssertFileWritable(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("The file path cannot be null or whitespace.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file {filePath} does not exist.", filePath);

        // Do nothing, just checking if the file can be opened for writing.
        using var fs = File.OpenWrite(filePath);
        //fs.Close();
    }

    /// <summary>Checks if the specified path represents a directory. If it does not, an IOUtilsException is thrown.</summary>
    /// <param name="directory">The path to check.</param>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="directory"/> is null, empty, or consists of whitespace only.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if the <paramref name="directory"/> does not exist or is not a directory.</exception>
    public static void AssertIsDirectory(string directory)
    {
        if (string.IsNullOrWhiteSpace(directory))
            throw new ArgumentException("The file path cannot be null or whitespace.", nameof(directory));
        if (!IsDirectory(directory))
            throw new DirectoryNotFoundException($"The path '{directory} is not a directory.");
    }

    /// <summary>Checks if the specified path represents a file. If it does not, a FileNotFoundException is thrown.</summary>
    /// <param name="filePath">The path to check.</param>
    /// <exception cref="FileNotFoundException">Thrown if the file specified by <paramref name="filePath"/> does not exist.</exception>
    public static void AssertIsFile(string filePath)
    {
        if (IsFile(filePath))
            return;

        throw new FileNotFoundException("The file does not exist");
    }

    /// <summary>Ensures that the parent directories of the specified file path exist.</summary>
    /// <param name="filePath">The file path for which to ensure the existence of parent directories.</param>
    /// <returns>The original file path, or the specified directory path if it already exists.</returns>
    public static string EnsureParentDirectoriesExist(string filePath)
    {
        if (IsDirectory(filePath))
            return filePath;

        var parentDirectory = Path.GetDirectoryName(filePath);
        Directory.CreateDirectory(parentDirectory);

        return Path.GetDirectoryName(filePath);
    }

    /// <summary>Formats the specified byte size into a human-readable string representation.</summary>
    /// <param name="size">The size in bytes.</param>
    /// <returns>A human-readable string representation of the byte size.</returns>
    public static string FormatByteSize(BigInteger size)
    {
        if (size < 0)
            throw new ArgumentException("Size cannot be negative");

        return size switch
        {
            _ when size >= ONE_PB_BI => $"{Math.Round((decimal)size / (decimal)ONE_PB_BI, 2)} PB",
            _ when size >= ONE_TB_BI => $"{Math.Round((decimal)size / (decimal)ONE_TB_BI, 2)} TB",
            _ when size >= ONE_GB_BI => $"{Math.Round((decimal)size / (decimal)ONE_GB_BI, 2)} GB",
            _ when size >= ONE_MB_BI => $"{Math.Round((decimal)size / (decimal)ONE_MB_BI, 2)} MB",
            _ when size >= ONE_KB_BI => $"{Math.Round((decimal)size / (decimal)ONE_KB_BI, 2)} KB",
            _ => $"{size} Bytes"
        };
    }

    /// <summary>Formats the specified byte size into a human-readable string representation.</summary>
    /// <param name="size">The size in bytes.</param>
    /// <returns>A human-readable string representation of the byte size.</returns>
    public static string FormatByteSize(long size)
    {
        return FormatByteSize(new BigInteger(size));
    }

    /// <summary>Checks if the specified directory contains a file with the specified name.</summary>
    /// <param name="directory">The path of the directory to check.</param>
    /// <param name="fileName">The name of the file to look for.</param>
    /// <returns>Returns true if the directory contains a file with the specified name; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="directory"/> path is null, empty, or consists of whitespace only, or if the <paramref name="fileName"/> is null, empty, or consists of whitespace only.</exception>
    public static bool IsDirectoryContaining(string directory, string fileName)
    {
        if (string.IsNullOrWhiteSpace(directory))
            throw new ArgumentException("The directory path is null, empty or consists of whitespace only");
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("The file name is null, empty, or consists of whitespace only.");
        if (!Directory.Exists(directory))
            return false;

        var filePath = Path.Combine(directory, Path.GetFileName(fileName));

        return File.Exists(filePath);
    }

    /// <summary>Determines whether the specified path represents a directory.</summary>
    /// <param name="path">The path to check.</param>
    /// <returns>Returns true if the specified path represents a directory; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="path"/> is null, empty, or contains only whitespace characters.</exception>
    public static bool IsDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("The path is null, empty, or contains only whitespace characters");

        if (!Directory.Exists(path))
            return false;

        var attributes = File.GetAttributes(path);
        return (attributes & FileAttributes.Directory) == FileAttributes.Directory;
    }

    /// <summary>Determines whether the specified path represents a file.</summary>
    /// <param name="path">The path to check.</param>
    /// <returns>Returns true if the specified path represents a file; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="path"/> is null, empty, or contains only whitespace characters.</exception>
    public static bool IsFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("The path is null, empty, or contains only whitespace characters");

        return File.Exists(path);
    }

    /// <summary>Renames a file by changing its name.</summary>
    /// <param name="oldFileName">The current name of the file.</param>
    /// <param name="newFileName">The new name for the file.</param>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="oldFileName"/> or <paramref name="newFileName"/> is null or empty.</exception>
    public static void RenameFile(string oldFileName, string newFileName)
    {
        if (string.IsNullOrEmpty(oldFileName))
            throw new ArgumentException("Old file name cannot be null or empty.", nameof(oldFileName));

        if (string.IsNullOrEmpty(newFileName))
            throw new ArgumentException("New file name cannot be null or empty.", nameof(newFileName));

        File.Move(oldFileName, newFileName);
    }
}