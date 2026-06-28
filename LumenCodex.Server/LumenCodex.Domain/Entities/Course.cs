using LumenCodex.Domain.DTOs;

namespace LumenCodex.Domain.Entities;

public partial class Course
{
    public Guid CourseId { get; set; }
    public string CourseName { get; set; }
    public string RelativePath { get; set; }
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
    public Course(string courseName, string relativePath)
    {
        CourseId = Guid.NewGuid();
        
        CourseName = courseName;
        RelativePath = relativePath;
    }

    public GetCourseDto ToGetCourseDto()
    {
        return new GetCourseDto(CourseId, CourseName);
    }
}