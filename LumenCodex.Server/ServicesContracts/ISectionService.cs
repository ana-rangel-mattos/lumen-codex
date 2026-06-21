using Entities;

namespace ServicesContracts;

public interface ISectionService
{
    public Task<List<Section>> GetSectionsByCourseId(Guid courseId);
}