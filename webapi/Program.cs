using webapi.Model;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));  // Use the dynamic port
});
// Enable CORS with a policy to allow requests from your React app

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register GoogleSheetsHelper as a singleton.
builder.Services.AddSingleton<GoogleSheetsHelper>();
var app = builder.Build();

// Apply CORS policy to the app
app.UseCors("AllowReactApp");
app.Use(async (context, next) =>
{
    // Add custom headers
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "DELETE, POST, GET, OPTIONS");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With");

    // Proceed with the next middleware in the pipeline
    await next.Invoke();
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
