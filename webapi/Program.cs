using webapi.Model;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GoogleSheetsHelper>();

var app = builder.Build();

app.UseCors(policy =>
{
    policy.WithOrigins("http://localhost:3000") // Replace with your React app's URL during development
          .AllowAnyHeader()
          .AllowAnyMethod();
});

// Serve static files from wwwroot
app.UseStaticFiles();

// SPA fallback: Route unmatched requests to index.html
app.MapFallbackToFile("index.html");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
