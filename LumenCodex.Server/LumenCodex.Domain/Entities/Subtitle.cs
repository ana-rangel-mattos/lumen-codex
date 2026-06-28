namespace LumenCodex.Domain.Entities;

public partial class Subtitle
{
    public Guid SubtitleId { get; }
    public Guid LessonId { get; set; }
    public string SubtitleName { get; set; }
    public string Lang { get; set; }
    public string RootPath { get; set; }
    public SubsType SubsType { get; set; }
    public virtual Lesson Lesson { get; set; } = null!;

    public Subtitle(string subtitleName, string rootPath, string lang, SubsType subsType)
    {
        SubtitleId = Guid.NewGuid();
        
        SubtitleName = subtitleName;
        Lang = lang;
        RootPath = rootPath;
        SubsType = subsType;
    }
}