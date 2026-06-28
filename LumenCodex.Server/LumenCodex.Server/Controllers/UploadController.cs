using Microsoft.AspNetCore.Mvc;
using LumenCodex.ServicesContracts;

namespace LumenCodex.Server.Controllers;

[ApiController]
[Route("/api/courses/[controller]")]
public class UploadController : Controller
{
    private readonly IFileScanner _fileScanner;
    private readonly INoteService _noteService;
    
    public UploadController(IFileScanner fileScanner, INoteService noteService)
    {
        _fileScanner = fileScanner;
        _noteService = noteService;
    }

    public record UploadPathRequest(string RootPath);
    
    // Upload a single course to Db
    [HttpPost]
    [Route("single")]
    public async Task<IActionResult> UploadSingleCourse([FromBody] UploadPathRequest request)
    {
        if (string.IsNullOrEmpty(request.RootPath))
            return BadRequest("Course path is required");

        string? sanitizedPath = GetSanitizedPath(request.RootPath);
        string? actualPath = FindActualPathCaseInsensitive(sanitizedPath);

        if (actualPath is null || !Directory.Exists(actualPath))
            return BadRequest($"Course path does not exist inside container: {sanitizedPath}");
        
        await _fileScanner.UploadSingleCourseToDb(new DirectoryInfo(actualPath));
        
        return Created();
    }
    
    // Upload a bunch of courses to Db
    [HttpPost]
    [Route("bunch")]
    public async Task<IActionResult> UploadBunchOfCourses([FromBody] UploadPathRequest request)
    {
        if (string.IsNullOrEmpty(request.RootPath))
            return BadRequest("Courses folder path is required");

        string? sanitizedPath = GetSanitizedPath(request.RootPath);
        string? actualPath = FindActualPathCaseInsensitive(sanitizedPath);

        if (actualPath is null || !Directory.Exists(actualPath))
            return BadRequest($"Courses path does not exist inside container: {sanitizedPath}");
        
        await _fileScanner.UploadBunchOfCoursesToDb(new DirectoryInfo(actualPath));

        return Created();
    }

    private static string? FindActualPathCaseInsensitive(string? path)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path)) return null;
        if (Directory.Exists(path)) return path;

        const string baseMount = "/courses_root";

        if (!path.StartsWith(baseMount, StringComparison.OrdinalIgnoreCase))
        {
            if (!Directory.Exists(path)) return null;
            return path;
        }

        var remainingPath = path.Substring(baseMount.Length);
        var parts = remainingPath.Split(
            new[] {
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar 
            }, StringSplitOptions.RemoveEmptyEntries);

        string currentPath = baseMount;

        foreach (var part in parts)
        {
            try
            {
                var matches = Directory.GetDirectories(currentPath);
                var matchedDir = matches.FirstOrDefault(d =>
                    Path.GetFileName(d).Equals(part, StringComparison.OrdinalIgnoreCase));

                if (matchedDir is not null)
                {
                    currentPath = matchedDir;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scanning subdirectories at {currentPath}: {ex.Message}");
                return null;
            }
        }

        return currentPath;
    }

    private static string? GetSanitizedPath(string rootPath)
    {
        return rootPath.Trim('"').Trim();
    }
}