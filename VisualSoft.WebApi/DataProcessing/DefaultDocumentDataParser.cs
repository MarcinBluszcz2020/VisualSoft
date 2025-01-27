using System.Globalization;
using VisualSoft.WebApi.Common;
using VisualSoft.WebApi.Model;

namespace VisualSoft.WebApi.DataProcessing;

public class DefaultDocumentDataParser : IDocumentDataParser
{
	private const string HeaderLinePrefix = "H,";
	private const string ItemLinePrefix = "B,";
	private const string CommentLinePrefix = "C,";

	private const string DateFormat = "dd-mm-yyyy";

	public Result<Document[]> ParseDocumentData(string[] documentLines)
	{
		var documents = new List<Document>();

		DocumentHeader? currentDocumentHeader = null;
		List<DocumentItem> currentDocumentItems = new List<DocumentItem>();

		for (int lineIndex = 0; lineIndex < documentLines.Length; lineIndex++)
		{
			var line = documentLines[lineIndex];

			var lineType = GetLineType(line);

			if (lineType == DocumentLineType.Unknown)
			{
				return UnknowLineFailedResult(lineIndex);
			}

			if (lineType == DocumentLineType.Header)
			{
				if (currentDocumentHeader != null)
				{
					documents.Add(new Document
					{
						Header = currentDocumentHeader,
						Items = currentDocumentItems.ToArray()
					});
				}

				var headerParseResult = ParseHeader(line, lineIndex);

				if (!headerParseResult.IsSuccess)
				{
					return headerParseResult.ConvertWithoutData<Document[]>();
				}

				currentDocumentHeader = headerParseResult.Data;
				currentDocumentItems.Clear();
			}

			if (lineType == DocumentLineType.Item)
			{
				var lineParseResult = ParseItemLine(line, lineIndex);

				if (!lineParseResult.IsSuccess)
				{
					return lineParseResult.ConvertWithoutData<Document[]>();
				}

				currentDocumentItems.Add(lineParseResult.Data);
			}
		}

		//add last document
		if (currentDocumentHeader != null)
		{
			documents.Add(new Document
			{
				Header = currentDocumentHeader,
				Items = currentDocumentItems.ToArray()
			});
		}


		if (documents.Count == 0)
		{
			return Result.Failed<Document[]>("No documents present in passed data");
		}

		var emptyDocuments = documents.Where(x => x.Items.Length == 0);

		if (emptyDocuments.Any())
		{
			var emptyDocumentsNumbers = string.Join( ',',emptyDocuments.Select(x => x.Header.NumerDokumentu));

			return Result.Failed<Document[]>($"Some documents are empty (documentIds: {emptyDocumentsNumbers})");
		}

		return Result.Successful(documents.ToArray());
	}

	private Result<DocumentItem> ParseItemLine(string line, int lineIndex)
	{
		try
		{
			var lineValues = GetLineValues(line);

			var documentItem = new DocumentItem
			{
				KodProduktu = lineValues[0],
				NazwaProduktu = lineValues[1],
				Ilosc = decimal.Parse(lineValues[2], CultureInfo.InvariantCulture),
				CenaNetto = decimal.Parse(lineValues[3], CultureInfo.InvariantCulture),
				WartoscNetto = decimal.Parse(lineValues[4], CultureInfo.InvariantCulture),
				Vat = decimal.Parse(lineValues[5], CultureInfo.InvariantCulture),
				IloscPrzed = decimal.Parse(lineValues[6], CultureInfo.InvariantCulture),
				AvgPrzed = decimal.Parse(lineValues[7], CultureInfo.InvariantCulture),
				IloscPo = decimal.Parse(lineValues[8], CultureInfo.InvariantCulture),
				AvgPo = decimal.Parse(lineValues[9], CultureInfo.InvariantCulture),
				Grupa = lineValues[10]
			};

			return Result.Successful(documentItem);
		}
		catch
		{
			return Result.Failed<DocumentItem>($"Error during parsing document item line (lineIndex: {lineIndex})");
		}
	}

	private Result<DocumentHeader> ParseHeader(string line, int lineIndex)
	{
		try
		{
			var lineValues = GetLineValues(line);

			var documentHeader = new DocumentHeader
			{
				KodBA = lineValues[0],
				Typ = lineValues[1],
				NumerDokumentu = long.Parse(lineValues[2]),
				DataOperacji = DateOnly.ParseExact(lineValues[3], DateFormat, CultureInfo.InvariantCulture),
				NumerDniaDokumentu = long.Parse(lineValues[4]),
				KodKontrahenta = lineValues[5],
				NazwaKontrahenta = lineValues[6],
				NumerDokumentuZewnetrznego = lineValues[7], //must be string,
				DataDokumentuZewnetrznego = DateOnly.ParseExact(lineValues[8], DateFormat, CultureInfo.InvariantCulture),
				Netto = decimal.Parse(lineValues[9], CultureInfo.InvariantCulture),
				Vat = decimal.Parse(lineValues[10], CultureInfo.InvariantCulture),
				Brutto = decimal.Parse(lineValues[11], CultureInfo.InvariantCulture),
				F1 = decimal.Parse(lineValues[12], CultureInfo.InvariantCulture),
				F2 = decimal.Parse(lineValues[13], CultureInfo.InvariantCulture),
				F3 = decimal.Parse(lineValues[14], CultureInfo.InvariantCulture),
			};

			return Result.Successful(documentHeader);
		}
		catch
		{
			return Result.Failed<DocumentHeader>($"Error during parsing document header line (lineIndex: {lineIndex})");
		}
	}

	private Result<Document[]> UnknowLineFailedResult(int lineIndex)
	{
		return Result.Failed<Document[]>($"Invalid data format, unknow line (lineIndex: {lineIndex})");
	}

	private string[] GetLineValues(string line)
	{
		return line.Split(',', options: StringSplitOptions.TrimEntries).Skip(1).ToArray(); //skip 1 value - line prefix
	}

	private DocumentLineType GetLineType(string line)
	{
		if (line.StartsWith(HeaderLinePrefix))
		{
			return DocumentLineType.Header;
		}
		else if (line.StartsWith(ItemLinePrefix))
		{
			return DocumentLineType.Item;
		}
		else if (line.StartsWith(CommentLinePrefix))
		{
			return DocumentLineType.Comment;
		}

		return DocumentLineType.Unknown;
	}
}
