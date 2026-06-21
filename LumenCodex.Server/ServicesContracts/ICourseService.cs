using Entities;

namespace ServicesContracts;

public interface ICourseService
{
    public Task<List<Course>> GetCoursesFromDb(int pageSize, int page);
    public Task<Course?> GetCourseById(Guid courseId);
    public Task DeleteCourseById(Guid courseId);
    public Task<int> GetCoursesCount();
}