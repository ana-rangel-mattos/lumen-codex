using Microsoft.AspNetCore.Mvc;
using Entities;
using ServicesContracts;

namespace LumenCodex.Server.Controllers;

public class SectionController : Controller
{
    private readonly ISectionService _sectionService;
    
    public SectionController(ISectionService sectionService)
    {
        _sectionService = sectionService;
    }
    
    [HttpGet]
    [Route("/api/courses/{courseId:guid}/sections")]
    public async Task<IActionResult> Index(Guid courseId)
    {
        List<Section> sections = await _sectionService.GetSectionsByCourseId(courseId);

        return Json(new
        {
            sections = sections.Select(s => new
            {
                sectionId = s.SectionId,
                sectionName = s.SectionName
            })
        });
    }
}