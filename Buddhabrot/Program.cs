using Buddhabrot.Persistence.Contexts;
using Buddhabrot.Persistence.Interfaces;
using Buddhabrot.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

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
	builder.Services.AddSwaggerGen(options =>
	{
		options.SwaggerDoc("v1", new OpenApiInfo
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

		var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
	});
	builder.Host.UseSerilog((context, config) => config.WriteTo.Console()
													   .ReadFrom.Configuration(context.Configuration));
	builder.Services.AddAutoMapper(typeof(Buddhabrot.API.DTO.ParameterProfile));
	builder.Services.AddDbContext<BuddhabrotContext>(options =>
	{
		// Get the SQL credentials from user secrets. See:
		// https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows
		var connStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"))
		{
			Password = builder.Configuration["DbPassword"]
		};
		options.UseSqlServer(connStrBuilder.ConnectionString);
	}).AddDatabaseDeveloperPageExceptionFilter()
	.AddScoped<IPlotRepository, PlotRepository>();

	var app = builder.Build();
	var mapper = app.Services.GetService<AutoMapper.IMapper>();
	mapper?.ConfigurationProvider.AssertConfigurationIsValid();

	// TODO:: Replace this with migrations.
	using var scope = app.Services.CreateScope();
	{
		var context = scope.ServiceProvider.GetRequiredService<BuddhabrotContext>();
		context.Database.EnsureDeleted();
		context.Database.EnsureCreated();
	}	

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
