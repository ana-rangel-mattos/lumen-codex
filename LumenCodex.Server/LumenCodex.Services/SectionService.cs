using Microsoft.EntityFrameworkCore;
using LumenCodex.Domain.Entities;
using LumenCodex.Domain.Utils;
using LumenCodex.ServicesContracts;

namespace LumenCodex.Services;

public class SectionService : ISectionService
{
    private readonly LumenContext _context;

    public SectionService(LumenContext context)
    {
        _context = context;
    }

    public async Task<List<Section>> GetSectionsByCourseId(Guid courseId)
    {
        var naturalComparer = new NaturalStringComparer();
        
        var sections = await _context.Sections
            .Where(s => s.CourseId == courseId)
            .ToListAsync();

        sections = sections.OrderBy(s => s.SectionName, naturalComparer)
            .ToList();

        return sections;
    }
}