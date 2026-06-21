using Markdig;
using Microsoft.EntityFrameworkCore;
using Entities;
using ServicesContracts;

namespace Services;

public class LessonService : ILessonService
{
    private readonly LumenContext _context;
    
    public LessonService(LumenContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetLessonsBySectionId(Guid sectionId)
    {
        var lessons = await _context.Lessons
            .Where(l => l.SectionId == sectionId)
            .Include(l => l.Subtitles)
            .OrderBy(l => l.LessonName)
            .ToListAsync();

        return lessons;
    }
    
    public Task<Lesson?> GetLessonById(Guid lessonId)
    {
        var lesson = _context.Lessons
            .FirstOrDefaultAsync(l => l.LessonId == lessonId);

        return lesson;
    }

    public async Task<bool> MarkLessonAsCompleted(Guid lessonId, bool isCompleted)
    {
        Lesson? foundLesson = await GetLessonById(lessonId);

        if (foundLesson is null)
            return false;
        
        await _context.Lessons
            .Where(l => l.LessonId == lessonId)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(l => l.IsCompleted, isCompleted));

        return true;
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