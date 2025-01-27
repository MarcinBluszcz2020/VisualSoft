using System.Globalization;
using VisualSoft.WebApi.Common;
using VisualSoft.WebApi.Model;

namespace VisualSoft.WebApi.DataProcessing;

public class DefaultDocumentDataParser : IDocumentDataParser
{
	private const string HeaderLinePrefix = "H,";
	private const string ItemLinePrefix = "B,";
	private const string CommentLinePrefix = "C,";

	private const int HeaderValuesCount = 15;

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


		return Result.Successful(documents.ToArray());
	}

	private Result<DocumentItem> ParseItemLine(string line, int lineIndex)
	{
		return Result.Successful(new DocumentItem
		{

		});
	}

	private Result<DocumentHeader> ParseHeader(string line, int lineIndex)
	{
		try
		{
			var lineValues = line.Split(',', options: StringSplitOptions.TrimEntries).ToList();

			RemovePrefixFromLineValues(lineValues);

			var documentHeader = new DocumentHeader
			{
				KodBA = lineValues[0],
				Typ = lineValues[1],
				NumerDokumentu = long.Parse(lineValues[2]),
				DataOperacji = DateTime.ParseExact(lineValues[3], "dd-mm-yyyy", CultureInfo.InvariantCulture),
				NumerDniaDokumentu = long.Parse(lineValues[4]),
				KodKontrahenta = lineValues[5],
				NazwaKontrahenta = lineValues[6],
				NumerDokumentuZewnetrznego = lineValues[7],//must be string,
				DataDokumentuZewnetrznego = DateTime.ParseExact(lineValues[8], "dd-mm-yyyy", CultureInfo.InvariantCulture),
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

	private void RemovePrefixFromLineValues(List<string> lineValues)
	{
		lineValues.RemoveAt(0);
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
