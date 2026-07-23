using EstoqueLiaTattoo.Data;
using EstoqueLiaTattoo.Services;
using EstoqueLiaTattoo.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "Data"));

builder.Services.AddDbContext<EstoqueLiaTattooContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EstoqueLiaTattooContext")
    ?? throw new InvalidOperationException("Connection string 'EstoqueLiaTattooContext' not found.")));

builder.Services.AddControllers();

builder.Services.AddScoped<IMovimentacaoServico, MovimentacaoService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ITintaService, TintaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EstoqueLiaTattooContext>();
    context.Database.Migrate();
    await DemoDataSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("ReactAppPolicy");

app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
