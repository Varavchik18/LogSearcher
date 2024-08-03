using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class LogSearcherBenchmarks
{
    private const string TestFilePath = "benchmark_test_log.txt";
    private LogSearcherService _searcher;
    private static string[] logLines = Enumerable.Repeat(
        "Error: NullReferenceException occurred.\n" +
        "Info: User logged in successfully.\n" +
        "Warning: Low disk space.\n" +
        "Error: Failed to connect to database.\n" +
        "Critical: System crash due to unknown error.\n" +
        "Debug: Initializing application.\n" +
        "Trace: Entering function.\n" +
        "Verbose: Detailed output.",
        10000).ToArray(); // Create a large array to simulate large files

    [GlobalSetup]
    public void Setup()
    {
        File.WriteAllLines(TestFilePath, logLines);
        _searcher = new LogSearcherService();
    }

    [Benchmark]
    public async Task SearchErrorPattern()
    {
        await _searcher.SearchAsync(TestFilePath, "Error:*");
    }

    [Benchmark]
    public async Task SearchComplexPattern()
    {
        await _searcher.SearchAsync(TestFilePath, "(Error:* or Critical:*) and (unknown or database)");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<LogSearcherBenchmarks>();
    }
}
