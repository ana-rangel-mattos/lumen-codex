using Entities;

namespace ServicesContracts;

public interface ISubtitleService
{
    public Task<List<Subtitle>> GetSubtitlesByLessonId(Guid lessonId);
    public string GetSubtitleType(SubsType subsType);
}