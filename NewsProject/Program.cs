using Microsoft.EntityFrameworkCore;
using NewsProject.DataTransferObject;
using NewsProject.Models;
using NewsProject.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NewsDbContext>(options =>
                                       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IService<New, NewDto>, NewService>();
builder.Services.AddScoped<IService<Category, CategoryDto>, CategoryService>();
builder.Services.AddScoped<ISearchService, NewService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (ArgumentNullException ex)
    {
        context.Response.StatusCode = 404;
        logger.Error(ex, "NotFound");
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        logger.Error(ex, "ServerInternalError");

    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
