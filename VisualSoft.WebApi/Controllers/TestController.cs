using Microsoft.AspNetCore.Mvc;
using VisualSoft.WebApi.Authorization;
using VisualSoft.WebApi.DataProcessing;
using VisualSoft.WebApi.Extensions;
using VisualSoft.WebApi.Model;

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

		var fileLinesReadResult = file.GetTextLines();

		if (!fileLinesReadResult.IsSuccess)
		{
			return Problem(fileLinesReadResult.GetErrorMessage());
		}

		var documentParseResult = _documentDataParser.ParseDocumentData(fileLinesReadResult.Data);

		if (!documentParseResult.IsSuccess)
		{
			return BadRequest(documentParseResult.GetErrorMessage());
		}

		var response = GetResponse(x, fileLinesReadResult.Data, documentParseResult.Data);

		return Ok(response);
	}

	private TestResponse GetResponse(
		int x,
		string[] fileLines,
		Document[] documents)
	{
		var lineCount = fileLines.Length;
		var charCount = fileLines.Sum(x => x.Length);
		var sum = documents.Sum(x => x.Header.Brutto);
		var xcount = documents.Count(doc => doc.Items.Length > x);

		var allItems = documents.SelectMany(x => x.Items);

		var maxItemNetValue = allItems.Max(x => x.WartoscNetto);
		var maxNetValProducts = allItems
			.Where(x => x.WartoscNetto == maxItemNetValue)
			.Select(x => x.NazwaProduktu)
			.Distinct();

		var maxNetValProductsString = string.Join(',', maxNetValProducts);

		return new TestResponse
		{
			Documents = documents,
			CharCount = charCount,
			LineCount = lineCount,
			Sum = sum,
			XCount = xcount,
			ProductWithMaxNetValue = maxNetValProductsString
		};
	}
}
