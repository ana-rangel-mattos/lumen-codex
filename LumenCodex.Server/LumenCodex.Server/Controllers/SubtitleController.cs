using Microsoft.AspNetCore.Mvc;
using Entities;
using ServicesContracts;

namespace LumenCodex.Server.Controllers;

public class SubtitleController : Controller
{
    private readonly ISubtitleService _subtitleService;

    public SubtitleController(ISubtitleService subtitleService)
    {
        _subtitleService = subtitleService;
    }
    
    [HttpGet]
    [Route("/api/lessons/{lessonId:guid}/subtitles")]
    public async Task<IActionResult> GetSubtitles(Guid lessonId)
    {
        List<Subtitle> subtitles = await _subtitleService.GetSubtitlesByLessonId(lessonId);

        return Json(new
        {
            subtitles = subtitles.Select(s => new
            {
                subtitleId = s.SubtitleId,
                subtitleName = s.SubtitleName,
                languageCode = s.Lang,
                subtitleType = _subtitleService.GetSubtitleType(s.SubsType),
                rootPath = s.RootPath
            })
        });
    }
}