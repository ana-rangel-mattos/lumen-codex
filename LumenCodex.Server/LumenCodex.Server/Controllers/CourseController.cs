using Entities;
using Microsoft.AspNetCore.Mvc;
using Entities;
using ServicesContracts;

namespace LumenCodex.Server.Controllers;

public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    
    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    
    [HttpGet]
    [Route("/api/courses/")]
    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        List<Course> courses = await _courseService.GetCoursesFromDb(limit, page);
        int totalItems = await _courseService.GetCoursesCount();
        int totalPages = (int) Math.Ceiling(totalItems / (double) limit);

        bool hasNextPage = totalPages > page;
        bool hasPreviousPage = page > 1;

        int nextPage = page + 1;
        int previousPage = page - 1;
        
        return Json(new
        {
            data = courses.Select(c => new
            {
                courseId = c.CourseId,
                courseName = c.CourseName,
            }),
            meta = new
            {
                totalItems,
                totalPages,
                currentPage = page,
                pageSize = limit,
                offset = (page - 1) * limit,
            },
            links = new
            {
                self = $"/api/courses?page={page}&limit={limit}",
                next = hasNextPage ? $"/api/courses?page={nextPage}&limit={limit}" : null,
                previous = hasPreviousPage ? $"/api/courses?page={previousPage}&limit={limit}" : null
            }
        });
    }

    [HttpDelete]
    [Route("/api/courses/{courseId:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid courseId)
    {
        Course? foundCourse = await _courseService.GetCourseById(courseId);

        if (foundCourse is null)
            return NotFound($"Course Id {courseId} could not be found.");
        
        await _courseService.DeleteCourseById(courseId);

        return NoContent();
    }

    [HttpGet]
    [Route("/api/courses/{courseId:guid}")]
    public async Task<IActionResult> GetCourse(Guid courseId)
    {
        Course? foundCourse = await _courseService.GetCourseById(courseId);

        if (foundCourse is null)
            return NotFound($"Course Id {courseId} could not be found.");

        return Json(new
        {
            courseId = foundCourse.CourseId,
            courseName = foundCourse.CourseName,
        });
    }
}