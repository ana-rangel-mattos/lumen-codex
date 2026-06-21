using Entities;

namespace ServicesContracts;

public interface IFileScanner
{
    public string[] AllowedExtensions { get; }
    public Task UploadBunchOfCoursesToDb(DirectoryInfo coursesDirInfo);
    public Task UploadSingleCourseToDb(DirectoryInfo courseDirInfo);
}