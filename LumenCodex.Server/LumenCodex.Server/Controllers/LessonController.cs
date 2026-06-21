using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Entities;
using ServicesContracts;

namespace LumenCodex.Server.Controllers;

public class LessonController : Controller
{
    private readonly ILessonService _lessonService;
    private readonly IFileScanner _fileScanner;
    
    public LessonController(ILessonService lessonService, IFileScanner fileScanner)
    {
        _lessonService = lessonService;
        _fileScanner = fileScanner;
    }
    
    [HttpGet]
    [Route("/api/lessons/{sectionId:guid}")]
    public async Task<IActionResult> GetLessons(Guid sectionId)
    {
        List<Lesson> lessons = await _lessonService.GetLessonsBySectionId(sectionId);
        
        
        
        return Json(new
        {
            lessons = lessons.Select(l => new
            {
                lessonId = l.LessonId,
                lessonName = l.LessonName,
                lessonType = _lessonService.GetLessonType(l.LessonType),
                streamPath = $"/api/lessons/{l.LessonId}/video",
                isCompleted = l.IsCompleted,
                lessonDurationSeconds = l.DurationSeconds
            })
        });
    }

    [HttpGet]
    [Route("/api/lesson/{lessonId:guid}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        Lesson? lesson = await _lessonService.GetLessonById(lessonId);

        if (lesson is null)
            return NotFound($"Lesson Id {lessonId} could not be found.");

        bool isVideoLesson = ILessonService.VideoLessonTypes.Contains(lesson.LessonType);

        return Json(new
        {
            lessonId = lesson.LessonId,
            lessonName = lesson.LessonName,
            lessonType = isVideoLesson ? "video" : "text",
            streamPath = $"/api/lessons/{lesson.LessonId}/{(isVideoLesson ? "video" : "lesson")}",
            isCompleted = lesson.IsCompleted
        });
    }

    [HttpGet]
    [Route("/api/lessons/{lessonId:guid}/text")]
    public async Task<IActionResult> GetLessonText(Guid lessonId)
    {
        Lesson? lesson = await _lessonService.GetLessonById(lessonId);
        
        if (lesson is null)
            return NotFound($"Lesson Id {lessonId} was not found");

        string content = "";
        
        if (ILessonService.TextLessonTypes.Contains(lesson.LessonType))
        {
            content = await _lessonService.ConvertTextToHtml(new FileInfo(lesson.RootPath));
        }

        return Content(content, "text/html");
    }
    
    [HttpGet]
    [Route("/api/lessons/{lessonId:guid}/video")]
    public async Task<IActionResult> StreamLessonVideo(Guid lessonId)
    {
        Lesson? lesson = await _lessonService.GetLessonById(lessonId);

        if (lesson is null)
            return NotFound($"Lesson Id {lessonId} was not found");
        
        string lessonType = lesson.LessonType.ToString().ToLower();
        string lessonPath = lesson.RootPath;

        var stream = new FileStream(lessonPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        FileExtensionContentTypeProvider typeProvider = new FileExtensionContentTypeProvider();
        
        if (!typeProvider.TryGetContentType(lessonPath, out string? contentType))
        { 
            contentType = "application/octet-stream";
        }
        
        if (_fileScanner.AllowedExtensions.Contains(lessonType))
        { 
            return new FileStreamResult(stream, contentType) { EnableRangeProcessing = true };
        }
        
        return BadRequest($"Lesson type {lessonType} is not supported.");
    }

    [HttpPatch]
    [Route("/api/lessons/{lessonId:guid}")]
    public async Task<IActionResult> MarkLessonAsCompleted(Guid lessonId, [FromQuery] bool? isCompleted)
    {
        if (isCompleted is null)
            return BadRequest("Is Completed must be provided");

        bool hasUpdated = await _lessonService.MarkLessonAsCompleted(lessonId, isCompleted.Value);

        if (!hasUpdated)
            return NotFound($"Lesson Id {lessonId} was not found");

        return NoContent();
    }
}