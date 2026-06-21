namespace Entities;

public class Course
{
    public Guid CourseId { get; set; }
    public string CourseName { get; set; }
    public List<Section> Sections { get; set; }
    public string RelativePath { get; set; }

    public Course(string courseName, string relativePath)
    {
        CourseId = Guid.NewGuid();
        
        CourseName = courseName;
        RelativePath = relativePath;
        Sections = new List<Section>();
    }
}