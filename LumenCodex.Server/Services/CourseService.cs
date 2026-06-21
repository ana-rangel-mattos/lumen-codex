using Microsoft.EntityFrameworkCore;
using Entities;
using ServicesContracts;

namespace Services;

public class CourseService : ICourseService
{
    private readonly LumenContext _context;
    public CourseService(LumenContext context)
    {
        _context = context;
    }
    
    public async Task<List<Course>> GetCoursesFromDb(int pageSize, int pageNumber)
    {
        int offset = (pageNumber - 1) * pageSize;
        
        return await _context.Courses
            .OrderBy(c => c.CourseName)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<Course?> GetCourseById(Guid courseId)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);

        if (course is not null)
            return course;

        return null;
    }

    public Task DeleteCourseById(Guid courseId)
    {
        return _context.Courses.Where(c => c.CourseId == courseId).ExecuteDeleteAsync();
    }

    public Task<int> GetCoursesCount()
    {
        return _context.Courses.CountAsync();
    }
}