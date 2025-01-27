namespace VisualSoft.WebApi.Model;

public class DocumentItem
{
	public required string KodProduktu { get; init; }
	public required string NazwaProduktu { get; init; }
	public required decimal Ilosc { get; init; }
	public required decimal CenaNetto { get; init; }
	public required decimal WartoscNetto { get; init; }
	public required decimal Vat { get; init; }
	public required decimal IloscPrzed { get; init; }
	public required decimal AvgPrzed { get; init; }
	public required decimal IloscPo { get; init; }
	public required decimal AvgPo { get; init; }
	public required string Grupa { get; init; }


}
