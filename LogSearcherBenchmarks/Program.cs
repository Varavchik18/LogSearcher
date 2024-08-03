using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.IO;
using System.Threading.Tasks;

public class LogSearcherBenchmarks
{
    private const string RealLogFilePath = @"C:\Users\Alex Colba\source\repos\LogSearcher\LogSearcher\HDFS_2k.log";
    private LogSearcherService _searcher;

    [GlobalSetup]
    public void Setup()
    {
        _searcher = new LogSearcherService();
    }

    [Benchmark]
    public async Task SearchErrorPattern()
    {
        // Пошук простого патерна
        await _searcher.SearchAsync(RealLogFilePath, "WARN*");
    }

    [Benchmark]
    public async Task SearchComplexPattern()
    {
        for (int i = 0; i < 100; i++)
        {
            await _searcher.SearchAsync(RealLogFilePath, "(INF* or ER*) and (unknown or served)");
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
