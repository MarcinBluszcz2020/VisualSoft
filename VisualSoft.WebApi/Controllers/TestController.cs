using Microsoft.AspNetCore.Mvc;

namespace VisualSoft.WebApi.Controllers;

[ApiController]
[Route("/api")]
public class TestController : ControllerBase
{
	private readonly AuthorizationService _authorizationService;

	public TestController(AuthorizationService authorizationService)
	{
		_authorizationService = authorizationService;
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


		return Ok();
	}
}
