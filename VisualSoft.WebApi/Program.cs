using VisualSoft.WebApi.DataProcessing;

namespace VisualSoft.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();

		builder.Services.AddSingleton<AuthorizationService>();
		builder.Services.AddSingleton<IDocumentDataParser, DefaultDocumentDataParser>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}
