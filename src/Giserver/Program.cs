
using Giserver.Endpoints;
using Giserver.Services;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.AddConsole();


var services = builder.Services;
services.AddSingleton<SlpkService>();
services.AddScoped<IConfigServcie, ConfigService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapSlpkEndpoint();
app.Run();

