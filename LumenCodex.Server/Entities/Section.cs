namespace Entities;

public class Section
{
    public Guid SectionId { get; set; }
    public Guid CourseId { get; set; }
    public string SectionName { get; set; }
    public List<Lesson> Lessons { get; set; }
    
    public Section(string sectionName)
    {
        SectionId = Guid.NewGuid();
        
        SectionName = sectionName;
        Lessons = new List<Lesson>();
    }
}