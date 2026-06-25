using Microsoft.EntityFrameworkCore;
using Entities;
using ServicesContracts;

namespace Services;

public class FileScanner : IFileScanner, IDisposable
{
    public string[] AllowedExtensions { get; } = ["mp4", "ts", "mkv", "webm", "md", "html", "txt"];
    
    private readonly LumenContext _context;
    
    public FileScanner(LumenContext context)
    {
        _context = context;
    }
    
    public async Task UploadBunchOfCoursesToDb(DirectoryInfo coursesDirInfo)
    {
        List<Course> localCourses = await GetBunchOfCourses(coursesDirInfo.FullName);

        foreach (Course localCourse in localCourses)
        {
            var courseInfo = Path.Combine(coursesDirInfo.FullName, localCourse.RelativePath);
            await UploadSingleCourseToDb(new DirectoryInfo(courseInfo));
        }
    }
    
    public async Task UploadSingleCourseToDb(DirectoryInfo courseDirInfo)
    {
        Course localCourse = await GetSingleCourse(courseDirInfo);

        var dbCourse = await _context.Courses
            .Include(c => c.Sections)
            .ThenInclude(s => s.Lessons)
            .ThenInclude(l => l.Subtitles)
            .FirstOrDefaultAsync(c => c.RelativePath == localCourse.CourseName);

        if (dbCourse is null)
        {
            await _context.Courses.AddAsync(localCourse);
        }
        else
        {
            await UpdateExistentCourse(localCourse, dbCourse);
        }
        

        await _context.SaveChangesAsync();
    }
    
    private async Task UpdateExistentCourse(Course localCourse, Course dbCourse)
    {
        // Add sections that are not in the db
            foreach (Section localSection in localCourse.Sections)
            {
                string localSectionName = localSection.SectionName;

                if (dbCourse.Sections.Any(s => s.SectionName == localSectionName) == false)
                {
                    await _context.Sections.AddAsync(localSection);
                }
            }
            
            // Delete Sections that are no more present in the local files
            foreach (Section dbSection in dbCourse.Sections)
            {
                string dbSectionName = dbSection.SectionName;

                if (localCourse.Sections.Any(s => s.SectionName == dbSectionName) == false)
                {
                    var sectionId = dbSection.SectionId;
                    await _context.Sections
                        .Where(s => s.SectionId == sectionId)
                        .ExecuteDeleteAsync();
                }
            }
            
            // Delete lessons that are no more present
            foreach (Section dbSection in dbCourse.Sections)
            {
                var localSection = localCourse.Sections.Find(s => s.SectionName == dbSection.SectionName);
                
                foreach (Lesson dbLesson in dbSection.Lessons)
                {
                    string dbLessonName = dbLesson.LessonName;

                    if (localSection is not null && localSection.Lessons.Any(l => l.LessonName == dbLessonName) == false)
                    {
                        var lessonId = dbLesson.LessonId;
                        await _context.Lessons
                            .Where(l => l.LessonId == lessonId)
                            .ExecuteDeleteAsync();
                    }
                }
            }

            foreach (Section localSection in localCourse.Sections)
            {
                var dbSection = dbCourse.Sections.Find(s => s.SectionName == localSection.SectionName);

                foreach (Lesson localLesson in localSection.Lessons)
                {
                    string localLessonName = localLesson.LessonName;

                    if (dbSection is not null && dbSection.Lessons.Any(l => l.LessonName == localLessonName) == false)
                    {
                        await _context.Lessons.AddAsync(localLesson);
                    }
                }
            }
    }
    
    private async Task<List<Course>> GetBunchOfCourses(string rootPath)
    {
        List<Course> courses = new List<Course>();
        var rootDir = new DirectoryInfo(rootPath);
        
        foreach (var courseDir in rootDir.GetDirectories())
        {
            var newCourse = await GetSingleCourse(courseDir);
            courses.Add(newCourse);
        }

        return courses;
    }
    
    private async Task<Course> GetSingleCourse(DirectoryInfo courseDir)
    {
        Course newCourse = new Course(courseDir.Name, courseDir.Name);
        
        foreach (var sectionInfo in courseDir.GetDirectories())
        {
            var newSection = await GetSection(sectionInfo);
            
            newCourse.Sections.Add(newSection);
        }

        return newCourse;
    }

    private async Task<int> CalculateReadingTime(string filePath)
    {
        if (!File.Exists(filePath))
            return 0;

        string content = await File.ReadAllTextAsync(filePath);
        
        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            return 0;

        int wordCount = content.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

        return (int) Math.Ceiling(wordCount / 225.0);
    }

    private async Task<Section> GetSection(DirectoryInfo sectionInfo)
    {
        var newSection = new Section(
            sectionInfo.Name
        );
        
        foreach (var lessonInfo in sectionInfo.GetFiles())
        {
            if (lessonInfo.Name.StartsWith(".") || lessonInfo.Name.EndsWith(".zip") || 
                lessonInfo.Name.EndsWith(".rar"))
                continue;
            
            string ext = GetLessonExtension(lessonInfo.Name);
            if (ext.EndsWith(".srt") || ext.EndsWith(".vtt"))
                continue;

            Lesson newLesson = await GetLesson(lessonInfo, sectionInfo);
            newSection.Lessons.Add(newLesson);
        }

        return newSection;
    }

    private async Task<Lesson> GetLesson(FileInfo lessonInfo, DirectoryInfo sectionInfo)
    {
        Lesson newLesson = new Lesson(
            Path.GetFileNameWithoutExtension(lessonInfo.Name),
            lessonInfo.FullName,
            false,
            GetLessonType(lessonInfo.Name)
        );
        
        var videoLessons = new List<LessonType> { LessonType.Mkv, LessonType.Ts, LessonType.Mp4, LessonType.Webm };
        var textLessons = new List<LessonType> { LessonType.Txt, LessonType.Html, LessonType.Md };
        
        if (newLesson.LessonType != LessonType.Ts && videoLessons.Contains(newLesson.LessonType))
        {
            var ffprobe = new NReco.VideoInfo.FFProbe();
            ffprobe.FFProbeExeName = "ffprobe";
            
            var videoInfo = ffprobe.GetMediaInfo(newLesson.RootPath);
            newLesson.DurationSeconds = videoInfo.Duration.TotalSeconds;
        }
        else if (textLessons.Contains(newLesson.LessonType))
        {
            newLesson.DurationSeconds = await CalculateReadingTime(newLesson.RootPath) * 60;
        }

        string lessonWithoutExt = Path.GetFileNameWithoutExtension(lessonInfo.Name);

        var subtitleFiles = sectionInfo.GetFiles()
            .Where(f => 
                !f.Name.StartsWith(".") &&
                f.Extension == ".srt" || f.Extension == ".vtt" 
                && f.Name.StartsWith(lessonWithoutExt)
            );

        foreach (var subtitleFile in subtitleFiles)
        {
            SubsType type = subtitleFile.Extension.ToLower() == ".srt" ? SubsType.Srt : SubsType.Vtt;
            string remainingPart = subtitleFile.Name.Substring(lessonWithoutExt.Length);
            string[] components = remainingPart.Split(".", StringSplitOptions.RemoveEmptyEntries);

            string subLang = "en";

            if (components.Length > 1)
            {
                subLang = components[0].ToLower();
            }
            
            Subtitle newSubtitle = new Subtitle(subtitleFile.Name, subtitleFile.FullName, subLang, type);
            newLesson.Subtitles.Add(newSubtitle);
        }

        return newLesson;
    }
    
    private string GetLessonExtension(string filename)
    {
        return Path.GetExtension(filename);
    }
    
    private LessonType GetLessonType(string filename)
    {
        string fileExtension = Path.GetExtension(filename);

        return fileExtension switch
        {
            ".mp4" => LessonType.Mp4,
            ".ts" => LessonType.Ts,
            ".mkv" => LessonType.Mkv,
            ".webm" => LessonType.Webm,
            ".md" => LessonType.Md,
            ".html" => LessonType.Html,
            _ => LessonType.Txt
        };
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}