using System.ComponentModel.DataAnnotations;

namespace LumenCodex.Domain.DTOs;

public record MarkLessonAsComplete(
    [property: Required] Guid LessonId,
    [property: Required] bool IsCompleted
);