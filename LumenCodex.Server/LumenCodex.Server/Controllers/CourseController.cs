using LumenCodex.Domain.Common;
using LumenCodex.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using LumenCodex.Domain.Extensions;
using LumenCodex.Domain.Result;
using LumenCodex.ServicesContracts;

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
    public async Task<IActionResult> Index([FromQuery] CourseQueryFilter filter, CancellationToken cancellationToken)
    {
        PagedResponse<GetCourseDto> response = await _courseService.GetCoursesAsync(filter, cancellationToken);
        
        return Ok(response);
    }

    [HttpDelete]
    [Route("/api/courses/{courseId:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await _courseService.DeleteCourseById(courseId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: NoContent,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.CourseCourseDoesNotExist => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            });
    }

    [HttpGet]
    [Route("/api/courses/{courseId:guid}")]
    public async Task<IActionResult> GetCourse(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await _courseService.GetCourseById(courseId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.CourseCourseDoesNotExist => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            });
    }
}