using LumenCodex.Domain.DTOs;
using LumenCodex.Domain.Entities;
using LumenCodex.Domain.Result;

namespace LumenCodex.ServicesContracts;

public interface ILessonService
{
    public static readonly LessonType[] TextLessonTypes = { LessonType.Html, LessonType.Txt, LessonType.Md };
    public static readonly LessonType[] VideoLessonTypes = { LessonType.Mkv, LessonType.Mp4, LessonType.Webm };
    public Task<List<Lesson>> GetLessonsBySectionId(Guid sectionId, CancellationToken cancellationToken = default);
    public Task<Result<Lesson>> GetLessonById(Guid lessonId, CancellationToken cancellationToken = default);
    public Task<Result> MarkLessonAsCompleted(MarkLessonAsComplete request, CancellationToken cancellationToken = default);
    public Task<Result<Course>> GetCourseByLessonId(Guid lessonId, CancellationToken cancellationToken = default);
    public Task<string> ConvertTextToHtml(FileInfo filePath);
    public string GetLessonType(LessonType lessonType);
}