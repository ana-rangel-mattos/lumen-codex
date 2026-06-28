namespace LumenCodex.Domain.Entities;

public partial class Lesson
{
    public Guid LessonId { get; set; }
    public Guid SectionId { get; set; }
    public string LessonName { get; set; }
    public bool IsCompleted { get; set; }
    public string RootPath { get; set; }
    public LessonType LessonType { get; set; }
    public double? DurationSeconds { get; set; }
    public double LastPositionSeconds { get; set; } = 0;
    public virtual ICollection<Subtitle> Subtitles { get; set; } = new List<Subtitle>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    public virtual Section Section { get; set; } = null!;

    public Lesson(string lessonName, string rootPath, bool isCompleted, LessonType lessonType)
    {
        LessonId = Guid.NewGuid();
        
        LessonName = lessonName;
        RootPath = rootPath;
        IsCompleted = isCompleted;
        LessonType = lessonType;
    }
}