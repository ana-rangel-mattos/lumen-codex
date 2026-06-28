namespace LumenCodex.Domain.Entities;

public partial class Section
{
    public Guid SectionId { get; set; }
    public Guid CourseId { get; set; }
    public string SectionName { get; set; }
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public virtual Course Course { get; set; } = null!;
    
    public Section(string sectionName)
    {
        SectionId = Guid.NewGuid();
        
        SectionName = sectionName;
    }
}