using LumenCodex.Domain.Common;
using LumenCodex.Domain.DTOs;
using LumenCodex.Domain.Entities;
using LumenCodex.Domain.Result;
using Microsoft.EntityFrameworkCore;
using LumenCodex.ServicesContracts;

namespace LumenCodex.Services;

public class CourseService : ICourseService
{
    private readonly LumenContext _context;
    public CourseService(LumenContext context)
    {
        _context = context;
    }
    
    public async Task<PagedResponse<GetCourseDto>> GetCoursesAsync(CourseQueryFilter filter, CancellationToken cancellationToken = default)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = _context.Courses.AsNoTracking().AsQueryable();

        query = query.ApplySearch(filter.SearchTerm);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySort(string.IsNullOrWhiteSpace(filter.SortBy) ? "CourseName" : filter.SortBy);

        var courses = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(c => new GetCourseDto(c.CourseId, c.CourseName))
            .ToListAsync(cancellationToken);

        return new PagedResponse<GetCourseDto>
        {
            Data = courses,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
    }
    
    public async Task<Result<GetCourseDto>> GetCourseById(Guid courseId, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses
            .Where(c => c.CourseId == courseId)
            .FirstOrDefaultAsync(cancellationToken);

        if (course is null)
        {
            return Result.Failure<GetCourseDto>(Errors.CourseWasNotFound(courseId));
        }

        return Result.Success(course.ToGetCourseDto());
    }

    public async Task<Result> DeleteCourseById(Guid courseId, CancellationToken cancellationToken = default)
    {
        int deletedRows = await _context.Courses
            .Where(c => c.CourseId == courseId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows > 0)
        {
            return Result.Success();
        }
        
        return Result.Failure(Errors.CourseWasNotFound(courseId));
    }

    public async Task<int> GetCompleteLessonsCount(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.Include(c => c.Sections)
            .ThenInclude(s => s.Lessons)
            .Where(c => c.CourseId == courseId && c
                .Sections
                .SelectMany(s => s.Lessons)
                .All(l => l.IsCompleted))
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetLessonsCount(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses.Include(c => c.Sections)
            .ThenInclude(s => s.Lessons)
            .Where(c => c.CourseId == courseId)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetCoursesCount(CancellationToken cancellationToken = default)
    {
        return await _context.Courses.CountAsync(cancellationToken);
    }
}