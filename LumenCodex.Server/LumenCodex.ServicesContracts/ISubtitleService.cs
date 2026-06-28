using LumenCodex.Domain.Entities;

namespace LumenCodex.ServicesContracts;

public interface ISubtitleService
{
    public Task<List<Subtitle>> GetSubtitlesByLessonId(Guid lessonId);
    public string GetSubtitleType(SubsType subsType);
}