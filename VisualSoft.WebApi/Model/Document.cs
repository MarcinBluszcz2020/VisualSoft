namespace VisualSoft.WebApi.Model;

public class Document
{
	public required DocumentHeader Header { get; init; }
	public required DocumentItem[] Items { get; init; }
}
