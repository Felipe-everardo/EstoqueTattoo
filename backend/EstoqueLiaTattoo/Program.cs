using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.Services;
using EstoqueLiaTattoo.Services.Impl;
var builder = WebApplication.CreateBuilder(args);

Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "Data"));

builder.Services.AddDbContext<EstoqueLiaTattooContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EstoqueLiaTattooContext")
    ?? throw new InvalidOperationException("Connection string 'EstoqueLiaTattooContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IMovimentacaoServico, MovimentacaoService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ITintaService, TintaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("ReactAppPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
