using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class LogSearchController : Controller
{
    private readonly LogSearcherService _logSearcherService;

    public LogSearchController()
    {
        _logSearcherService = new LogSearcherService();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(IFormFile fileUpload, string searchPattern)
    {
        if (fileUpload == null)
        {
            ViewBag.Error = "File must be uploaded.";
            return View("Index");
        }

        // Save the uploaded file to a temporary location
        var tempFilePath = Path.GetTempFileName();
        using (var stream = System.IO.File.Create(tempFilePath))
        {
            await fileUpload.CopyToAsync(stream);
        }

        TempData["UploadedFilePath"] = tempFilePath;

        IEnumerable<string> results;

        if (string.IsNullOrWhiteSpace(searchPattern))
        {
            // If no search pattern is provided, read all lines
            results = await System.IO.File.ReadAllLinesAsync(tempFilePath);
        }
        else
        {
            // Perform the search using the LogSearcherService
            results = await _logSearcherService.SearchAsync(tempFilePath, searchPattern);
        }

        ViewBag.Results = results;
        ViewBag.SearchPattern = searchPattern;

        return View("Index");
    }

    public async Task<IActionResult> ViewLog(string filePath, string lineContent)
    {
        if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(lineContent))
        {
            ViewBag.Error = "File path and line content cannot be empty.";
            return View("Index");
        }

        var lines = await System.IO.File.ReadAllLinesAsync(filePath);
        int lineNumber = Array.IndexOf(lines, lineContent);

        ViewBag.FilePath = filePath;
        ViewBag.LineContent = lineContent;
        ViewBag.LineNumber = lineNumber;
        ViewBag.Lines = lines;
        return View();
    }
}
