namespace VisualSoft.WebApi.Model;

public class DocumentHeader
{
	public required string KodBA { get; init; } 
	public required string Typ { get; init; }

	public required int NumerDokumentu { get; init; }
	public required DateTime DataOperacji { get; init; }
	public required int NumerDniaDokumentu { get; init; }

	public required string KodKontrahenta { get; init; }
	public required string NazwaKontrahenta { get; init; }

	public required int NumerDokumentuZewnetrznego { get; init; }
	public required DateTime DataDokumentuZewnetrznego { get; init; }

	public required double Netto { get; init; }
	public required double Vat { get; init; }
	public required double Brutto { get; init; }

	public required double F1 { get; init; }
	public required double F2 { get; init; }
	public required double F3 { get; init; }
}
