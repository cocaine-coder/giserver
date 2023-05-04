
using Giserver.Endpoints;
using Giserver.Services;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddSingleton<ZipService>();

var app = builder.Build();

app.MapSlpkEndpoint();
app.Run();

