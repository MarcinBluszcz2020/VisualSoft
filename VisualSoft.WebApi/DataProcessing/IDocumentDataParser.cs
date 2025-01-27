using VisualSoft.WebApi.Common;
using VisualSoft.WebApi.Model;

namespace VisualSoft.WebApi.DataProcessing;

public interface IDocumentDataParser
{
	Result<Document[]> ParseDocumentData(string[] documentLines);
}
