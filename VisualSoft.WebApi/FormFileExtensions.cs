namespace Microsoft.AspNetCore.Http;

public static class FormFileExtensions
{
	public static List<string> GetTextLines(this IFormFile file)
	{
		var fileLines = new List<string>();

		using (var fileStream = file.OpenReadStream())
		{
			using (var streamReader = new StreamReader(fileStream))
			{
				while (true)
				{
					var line = streamReader.ReadLine();

					if (line != null)
					{
						fileLines.Add(line);
					}
					else
					{
						break;
					}
				}
			}
		}

		return fileLines;
	}
}
