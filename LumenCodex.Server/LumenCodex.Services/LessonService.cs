using LumenCodex.Domain.DTOs;
using LumenCodex.Domain.Entities;
using LumenCodex.Domain.Result;
using LumenCodex.Domain.Utils;
using Markdig;
using Microsoft.EntityFrameworkCore;
using LumenCodex.ServicesContracts;

namespace LumenCodex.Services;

public class LessonService : ILessonService
{
    private readonly LumenContext _context;
    
    public LessonService(LumenContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetLessonsBySectionId(Guid sectionId, CancellationToken cancellationToken = default)
    {
        var naturalComparer = new NaturalStringComparer();
        
        var lessons = await _context.Lessons
            .Where(l => l.SectionId == sectionId)
            .Include(l => l.Subtitles)
            .ToListAsync(cancellationToken);

        var orderedLessons = lessons
            .OrderBy(l => l.LessonName, naturalComparer)
            .ToList();

        return orderedLessons;
    }
    
    public async Task<Result<Lesson>> GetLessonById(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var lesson = await _context.Lessons
            .Where(l => l.LessonId == lessonId)
            .FirstOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            return Result.Failure<Lesson>(Errors.LessonWasNotFound(lessonId));
        }

        return Result.Success(lesson);
    }

    public async Task<Result> MarkLessonAsCompleted(MarkLessonAsComplete request, CancellationToken cancellationToken = default)
    {
        var foundLesson = await _context.Lessons
            .Where(l => l.LessonId == request.LessonId)
            .FirstOrDefaultAsync(cancellationToken);

        if (foundLesson is null)
            return Result.Failure(Errors.LessonWasNotFound(request.LessonId));
        
        var updatedRows = await _context.Lessons
            .Where(l => l.LessonId == request.LessonId)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(l => l.IsCompleted, request.IsCompleted), cancellationToken);

        if (updatedRows > 0)
        {
            return Result.Success();
        }
        
        return Result.Failure(Errors.LessonWasNotFound(request.LessonId));
    }

    public async Task<string> ConvertTextToHtml(FileInfo filePath)
    {
        string? content = null;
        using (StreamReader sr = new StreamReader(filePath.FullName))
        {
            content = await sr.ReadToEndAsync();
        }

        if (string.IsNullOrEmpty(content))
        {
            return "";
        }
        
        var result = Markdown.ToHtml(content);
        
        return result;
    }

    public async Task<Result<Course>> GetCourseByLessonId(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var lesson = await _context.Lessons
            .Include(l => l.Section)
            .ThenInclude(s => s.Course)
            .Where(l => l.LessonId == lessonId)
            .FirstOrDefaultAsync(cancellationToken);

        if (lesson is null)
        {
            return Result.Failure<Course>(Errors.LessonWasNotFound(lessonId));
        }

        return Result.Success(lesson.Section.Course);
    }

    public string GetLessonType(LessonType lessonType)
    {
        return lessonType switch
        {
            LessonType.Html => "html",
            LessonType.Md => "markdown",
            LessonType.Mp4 => "mp4",
            LessonType.Mkv => "mkv",
            LessonType.Txt => "txt",
            LessonType.Webm => "webm",
            _ => "unknown"
        };
    }
}