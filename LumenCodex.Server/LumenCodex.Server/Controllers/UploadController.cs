using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Entities;
using ServicesContracts;

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
    
    // Upload a single course to Db
    [HttpPost]
    [Route("single")]
    public async Task<IActionResult> UploadSingleCourse([FromForm] string rootPath)
    {
        if (string.IsNullOrEmpty(rootPath))
            return BadRequest("Course path is required");

        if (!Directory.Exists(rootPath))
            return BadRequest("Course path does not exist");
        
        await _fileScanner.UploadSingleCourseToDb(new DirectoryInfo(rootPath));
        
        return Created();
    }
    
    // Upload a bunch of courses to Db
    [HttpPost]
    [Route("bunch")]
    public async Task<IActionResult> UploadBunchOfCourses([FromForm] string rootPath)
    {
        if (string.IsNullOrEmpty(rootPath))
            return BadRequest("Courses folder path is required");

        if (!Directory.Exists(rootPath))
            return BadRequest("Courses folder path does not exist");
        
        await _fileScanner.UploadBunchOfCoursesToDb(new DirectoryInfo(rootPath));

        return Created();
    }
}