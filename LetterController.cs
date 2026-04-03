using LetterGenApp.Models;
using LetterGenApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LetterGenApp.Controllers;

public class LetterController : Controller
{
    private readonly ILetterService _letterService;

    public LetterController(ILetterService letterService)
    {
        _letterService = letterService;
    }

    [HttpGet("/")]
    public IActionResult Index() => View(new LetterRequest());

    [HttpPost("/generate")]
    public async Task<IActionResult> Generate(LetterRequest request)
    {
        if (!ModelState.IsValid)
            return View("Index", request);

        var result = await _letterService.GenerateLetterAsync(request);
        return View("Result", result);
    }

    [HttpPost("/regenerate")]
    public async Task<IActionResult> Regenerate(LetterRequest request)
    {
        var result = await _letterService.GenerateLetterAsync(request);
        return View("Result", result);
    }
}
