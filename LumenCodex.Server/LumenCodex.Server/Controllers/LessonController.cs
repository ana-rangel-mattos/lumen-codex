using LumenCodex.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using LumenCodex.Domain.Entities;
using LumenCodex.Domain.Extensions;
using LumenCodex.Domain.Result;
using LumenCodex.Domain.Utils;
using LumenCodex.ServicesContracts;

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
    
    [HttpGet("/api/lessons/{sectionId:guid}")]
    public async Task<IActionResult> GetLessons(Guid sectionId, CancellationToken cancellationToken)
    {
        List<Lesson> lessons = await _lessonService.GetLessonsBySectionId(sectionId, cancellationToken);
        
        return Ok(new
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

    [HttpGet("/api/lesson/{lessonId:guid}")]
    public async Task<IActionResult> GetLesson(Guid lessonId, CancellationToken cancellationToken)
    {
        var lessonResult = await _lessonService.GetLessonById(lessonId, cancellationToken);

        if (lessonResult.IsFailure)
        {
            return lessonResult.Match<IActionResult>(
                onSuccess: Ok,
                onFailure: error =>
                {
                    return error.Code switch
                    {
                        ErrorNames.LessonLessonDoesNotExist => NotFound(error),
                        _ => StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.")
                    };
                });
        }

        var courseResult = await _lessonService.GetCourseByLessonId(lessonId, cancellationToken);
        
        return courseResult.Match<IActionResult>(
            onSuccess: () =>
            {
                var course = courseResult.Value;
                var naturalComparer = new NaturalStringComparer();

                var fullCourseLessons = course.Sections
                    .OrderBy(s => s.SectionName, naturalComparer)
                    .SelectMany(s => s.Lessons.OrderBy(l => l.LessonName, naturalComparer))
                    .ToList();

                int currentIndex = fullCourseLessons.FindIndex(l => l.LessonId == lessonId);
                
                var lesson = lessonResult.Value;
                bool isVideoLesson = ILessonService.VideoLessonTypes.Contains(lesson.LessonType);
                return Ok(new
                {
                    lessonId = lesson.LessonId,
                    lessonName = lesson.LessonName,
                    lessonType = isVideoLesson ? "video" : "text",
                    streamPath = $"/api/lessons/{lesson.LessonId}/{(isVideoLesson ? "video" : "lesson")}",
                    isCompleted = lesson.IsCompleted,
                    previousLessonId = currentIndex > 0 ? fullCourseLessons[currentIndex - 1]?.LessonId : null,
                    nextLessonId = currentIndex < fullCourseLessons.Count - 1 ? fullCourseLessons[currentIndex + 1]?.LessonId : null,
                });
            },
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.LessonLessonDoesNotExist => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.")
                };
            });
    }

    [HttpGet("/api/lessons/{lessonId:guid}/text")]
    public async Task<IActionResult> GetLessonText(Guid lessonId, CancellationToken cancellationToken)
    {
        var lessonResult = await _lessonService.GetLessonById(lessonId, cancellationToken);

        if (lessonResult.IsFailure)
        {
            return lessonResult.Match<IActionResult>(
                onSuccess: Ok,
                onFailure: error =>
                {
                    return error.Code switch
                    {
                        ErrorNames.LessonLessonDoesNotExist => NotFound(error),
                        _ => StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.")
                    };
                }
            );   
        }

        var lesson = lessonResult.Value;
        string content = "";
    
        if (ILessonService.TextLessonTypes.Contains(lesson.LessonType))
        {
            content = await _lessonService.ConvertTextToHtml(new FileInfo(lesson.RootPath));
        }

        return Content(content, "text/html");
    }
    
    [HttpGet("/api/lessons/{lessonId:guid}/video")]
    public async Task<IActionResult> StreamLessonVideo(Guid lessonId, CancellationToken cancellationToken)
    {
        var lessonResult = await _lessonService.GetLessonById(lessonId, cancellationToken);

        if (lessonResult.IsFailure)
        {
            return lessonResult.Match<IActionResult>(
                onSuccess: Ok,
                onFailure: error =>
                {
                    return error.Code switch
                    {
                        ErrorNames.LessonLessonDoesNotExist => NotFound(error),
                        _ => StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.")
                    };
                }
            );   
        }

        var lesson = lessonResult.Value;
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

    [HttpPatch("/api/lessons/{lessonId:guid}")]
    public async Task<IActionResult> MarkLessonAsCompleted(Guid lessonId, [FromQuery] bool isCompleted, CancellationToken cancellationToken)
    {
        MarkLessonAsComplete request = new(lessonId, isCompleted);
        
        var result = await _lessonService.MarkLessonAsCompleted(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.LessonLessonDoesNotExist => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error.")
                };
            });
    }
}