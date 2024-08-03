class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Please enter the path to the log file:");
        string filePath = Console.ReadLine();

        Console.WriteLine("Please enter the search pattern:");
        string searchPattern = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(searchPattern))
        {
            Console.WriteLine("File path and search pattern cannot be empty.");
            return;
        }

        var searcher = new LogSearcherService();
        var results = await searcher.SearchAsync(filePath, searchPattern);

        Console.WriteLine($"Found {results.Count()} matching lines:");
        foreach (var line in results)
        {
            Console.WriteLine(line);
        }
    }
}
