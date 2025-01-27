namespace VisualSoft.WebApi.Model;

public class DocumentHeader
{
	public required string KodBA { get; init; } 
	public required string Typ { get; init; }

	public required long NumerDokumentu { get; init; }
	public required DateOnly DataOperacji { get; init; }
	public required long NumerDniaDokumentu { get; init; }

	public required string KodKontrahenta { get; init; }
	public required string NazwaKontrahenta { get; init; }

	public required string NumerDokumentuZewnetrznego { get; init; }
	public required DateOnly DataDokumentuZewnetrznego { get; init; }

	public required decimal Netto { get; init; }
	public required decimal Vat { get; init; }
	public required decimal Brutto { get; init; }

	public required decimal F1 { get; init; }
	public required decimal F2 { get; init; }
	public required decimal F3 { get; init; }
}
