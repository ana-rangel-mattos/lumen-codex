using LumenCodex.Domain.Entities;

namespace LumenCodex.ServicesContracts;

public interface ISectionService
{
    public Task<List<Section>> GetSectionsByCourseId(Guid courseId);
}