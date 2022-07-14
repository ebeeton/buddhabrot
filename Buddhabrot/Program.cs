using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

Log.Information("Starting up...");

try
{
	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container.

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(c =>
	{
		c.SwaggerDoc("v1", new OpenApiInfo
		{
			Version = "v1",
			Title = "Buddhabrot API",
			Description = "A demo project to plot Buddhabrot images.",
			Contact = new OpenApiContact
			{
				Name = "Evan Beeton",
				Url = new Uri("https://github.com/ebeeton")
			}
		});
	});
	builder.Host.UseSerilog((context, config) => config.WriteTo.Console()
													   .ReadFrom.Configuration(context.Configuration));
	builder.Services.AddAutoMapper(typeof(Buddhabrot.API.DTO.ParameterProfile));

	var app = builder.Build();
	var mapper = app.Services.GetService<AutoMapper.IMapper>();
	mapper!.ConfigurationProvider.AssertConfigurationIsValid();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseSerilogRequestLogging();

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Unhandled exception.");
}
finally
{
	Log.Information("Shutting down gracefully.");
	Log.CloseAndFlush();
}
