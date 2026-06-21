namespace Entities;

public class Note
{
    public Guid NoteId { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public double PositionSeconds { get; set; }

    public Note(string title, string content, double positionSeconds)
    {
        NoteId = Guid.NewGuid();
        
        Title = title;
        Content = content;
        PositionSeconds = positionSeconds;
    }
}