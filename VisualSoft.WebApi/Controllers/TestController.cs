using Microsoft.AspNetCore.Mvc;
using VisualSoft.WebApi.DataProcessing;

namespace VisualSoft.WebApi.Controllers;

[ApiController]
[Route("/api")]
public class TestController : ControllerBase
{
	private readonly AuthorizationService _authorizationService;
	private readonly IDocumentDataParser _documentDataParser;

	public TestController(
		AuthorizationService authorizationService,
		IDocumentDataParser documentDataParser)
	{
		_authorizationService = authorizationService;
		_documentDataParser = documentDataParser;
	}

	[HttpPost("test/{x}")]
	public IActionResult Test([FromRoute] int x, [FromForm] IFormFile file)
	{
		var authorized = _authorizationService.Authorize(HttpContext);

		if (!authorized)
		{
			return new StatusCodeResult(StatusCodes.Status401Unauthorized);
		}

		var fileLines = file.GetTextLines();

		var documentParseResult = _documentDataParser.ParseDocumentData(fileLines);

		return Ok();
	}
}
