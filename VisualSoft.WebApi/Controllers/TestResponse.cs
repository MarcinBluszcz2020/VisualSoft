using VisualSoft.WebApi.Model;

namespace VisualSoft.WebApi.Controllers;

public class TestResponse
{
	public required Document[] Documents { get; init; }
	public required long LineCount { get; init; }
	public required long CharCount { get; init; }
	public required decimal Sum { get; init; }
	public required long XCount { get; init; }
	public required string ProductWithMaxNetValue { get; init; }
}
