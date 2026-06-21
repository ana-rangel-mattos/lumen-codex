using Microsoft.EntityFrameworkCore;
using Entities;
using ServicesContracts;

namespace Services;

public class SectionService : ISectionService
{
    private readonly LumenContext _context;
    
    public SectionService(LumenContext context)
    {
        _context = context;
    }
    
    public async Task<List<Section>> GetSectionsByCourseId(Guid courseId)
    {
        var sections = await _context.Sections
            .Where(s => s.CourseId == courseId)
            .OrderBy(s => s.SectionName)
            .ToListAsync();

        return sections;
    }
}