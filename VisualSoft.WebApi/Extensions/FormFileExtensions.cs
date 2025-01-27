using VisualSoft.WebApi.Common;

namespace VisualSoft.WebApi.Extensions;

public static class FormFileExtensions
{
    public static Result<string[]> GetTextLines(this IFormFile file)
    {
        try
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

            return Result.Successful( fileLines.ToArray());
        }
        catch
        {
            return Result.Failed<string[]>("Cannot read file content");
        }
    }
}
