using Entities;

namespace ServicesContracts;

public interface ILessonService
{
    public static readonly LessonType[] TextLessonTypes = { LessonType.Html, LessonType.Txt, LessonType.Md };
    public static readonly LessonType[] VideoLessonTypes = { LessonType.Mkv, LessonType.Mp4, LessonType.Webm };
    public Task<List<Lesson>> GetLessonsBySectionId(Guid sectionId);
    public Task<Lesson?> GetLessonById(Guid lessonId);
    public Task<bool> MarkLessonAsCompleted(Guid lessonId, bool isCompleted);
    public Task<string> ConvertTextToHtml(FileInfo filePath);
    public string GetLessonType(LessonType lessonType);
}