using FluentAssertions;

public class LogSearcherTests
{
    private const string TestFilePath = "test_log.txt";

    [Fact]
    public async Task SearchAsync_ShouldFindMatchingLines()
    {
        // Arrange
        string[] logLines = {
            "Error: NullReferenceException occurred.",
            "Info: User logged in successfully.",
            "Warning: Low disk space.",
            "Error: Failed to connect to database."
        };

        await File.WriteAllLinesAsync(TestFilePath, logLines);
        var searcher = new LogSearcherService();

        // Act
        var results = await searcher.SearchAsync(TestFilePath, "Error:*");

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(new[] {
            "Error: NullReferenceException occurred.",
            "Error: Failed to connect to database."
        });

        // Clean up
        File.Delete(TestFilePath);
    }


}
