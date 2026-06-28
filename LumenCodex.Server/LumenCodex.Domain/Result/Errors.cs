namespace LumenCodex.Domain.Result;

public static class Errors
{
    public static Error LessonWasNotFound(Guid lessonId) => new Error(
        ErrorNames.LessonLessonDoesNotExist,
        $"Lesson with ID {lessonId} was not found!");

    public static Error CourseWasNotFound(Guid courseId) => new Error(
        ErrorNames.CourseCourseDoesNotExist,
        $"Course with ID {courseId} was not found!"
        );
}