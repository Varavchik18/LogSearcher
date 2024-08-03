using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

public class LogSearcherServiceWildcardTests
{
    private const string TestFilePath = "test_log.txt";

    public LogSearcherServiceWildcardTests()
    {
        // Arrange: Create a test log file with sample data
        string[] logLines = {
            "Error: NullReferenceException occurred.",
            "Info: User logged in successfully.",
            "Warning: Low disk space.",
            "Error: Failed to connect to database.",
            "Critical: System crash due to unknown error.",
            "Debug: Initializing application.",
            "Trace: Entering function.",
            "Verbose: Detailed output."
        };

        File.WriteAllLines(TestFilePath, logLines);
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportAsteriskWildcard()
    {
        // Arrange
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "Error:*");

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(new[] {
            "Error: NullReferenceException occurred.",
            "Error: Failed to connect to database."
        });
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportQuestionMarkWildcard()
    {
        // Arrange
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "Error: ????ReferenceException*");

        // Assert
        results.Should().HaveCount(1);
        results.First().Should().Be("Error: NullReferenceException occurred.");
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportAndOperator()
    {
        // Arrange
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "Error:* and database");

        // Assert
        results.Should().HaveCount(1);
        results.First().Should().Be("Error: Failed to connect to database.");
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportOrOperator()
    {
        // Arrange
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "Error:* or Critical:*");

        // Assert
        results.Should().HaveCount(3);
        results.Should().Contain(new[] {
            "Error: NullReferenceException occurred.",
            "Error: Failed to connect to database.",
            "Critical: System crash due to unknown error."
        });
    }

    [Fact]
    public async Task SearchAsync_ShouldSupportParenthesesForGrouping()
    {
        // Arrange
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "(Error:* or Critical:*) and unknown");

        // Assert
        results.Should().HaveCount(1);
        results.First().Should().Be("Critical: System crash due to unknown error.");
    }

    [Fact]
    public async Task SearchAsync_ShouldHandleComplexExpressions()
    {
        // Arrange
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "(Error:* or Critical:*) and (unknown or database)");

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(new[] {
            "Error: Failed to connect to database.",
            "Critical: System crash due to unknown error."
        });
    }

    public void Dispose()
    {
        // Clean up test file after each test
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }
}
