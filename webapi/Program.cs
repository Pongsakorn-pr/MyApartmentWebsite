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

app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
