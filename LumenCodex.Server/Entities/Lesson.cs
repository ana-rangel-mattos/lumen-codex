namespace Entities;

public class Lesson
{
    public Guid LessonId { get; set; }
    public Guid SectionId { get; set; }
    public string LessonName { get; set; }
    public bool IsCompleted { get; set; }
    public string RootPath { get; set; }
    public LessonType LessonType { get; set; }
    public List<Subtitle> Subtitles { get; set; }
    public List<Note> Notes { get; set; }

    public double? DurationSeconds { get; set; }
    public double LastPositionSeconds { get; set; } = 0;

    public Lesson(string lessonName, string rootPath, bool isCompleted, LessonType lessonType)
    {
        LessonId = Guid.NewGuid();
        
        LessonName = lessonName;
        RootPath = rootPath;
        IsCompleted = isCompleted;
        LessonType = lessonType;
        Subtitles = new List<Subtitle>();
        Notes = new List<Note>();
    }
}