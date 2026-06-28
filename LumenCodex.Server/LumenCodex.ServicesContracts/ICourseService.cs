using LumenCodex.Domain.Common;
using LumenCodex.Domain.DTOs;
using LumenCodex.Domain.Result;

namespace LumenCodex.ServicesContracts;

public interface ICourseService
{
    public Task<PagedResponse<GetCourseDto>> GetCoursesAsync(CourseQueryFilter filter, CancellationToken cancellationToken = default);
    public Task<Result<GetCourseDto>> GetCourseById(Guid courseId, CancellationToken cancellationToken = default);
    public Task<Result> DeleteCourseById(Guid courseId, CancellationToken cancellationToken = default);
    public Task<int> GetCoursesCount(CancellationToken cancellationToken = default);
    public Task<int> GetCompleteLessonsCount(Guid courseId, CancellationToken cancellationToken = default);
    public Task<int> GetLessonsCount(Guid courseId, CancellationToken cancellationToken = default);
}