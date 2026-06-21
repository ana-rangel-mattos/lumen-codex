using Microsoft.EntityFrameworkCore;
using Entities;
using ServicesContracts;

namespace Services;

public class SubtitleService : ISubtitleService
{
    private readonly LumenContext _context;

    public SubtitleService(LumenContext context)
    {
        _context = context;
    }
    
    public async Task<List<Subtitle>> GetSubtitlesByLessonId(Guid lessonId)
    {
        var subtitles = await _context.Subtitles
            .Where(s => s.LessonId == lessonId)
            .OrderBy(s => s.SubtitleName)
            .ToListAsync();

        return subtitles;
    }
    
    public string GetSubtitleType(SubsType subsType)
    {
        return subsType switch
        {
            SubsType.Srt => "srt",
            SubsType.Vtt => "vtt",
            _ => "unknown"
        };
    }
}