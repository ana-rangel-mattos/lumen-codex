namespace LumenCodex.Domain.Entities;

public partial class Note
{
    public Guid NoteId { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public double TimestampSeconds { get; set; }
    public virtual Lesson Lesson { get; set; } = null!;
}