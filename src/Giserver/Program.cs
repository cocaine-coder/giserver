using Npgsql.GeoQuery.Extensions;

using Giserver.Endpoints;
using Giserver.Services;

var builder = WebApplication.CreateSlimBuilder(args);
var configuration = builder.Configuration;
configuration.AddJsonFile("appsetting.json");

var services = builder.Services;
services.AddScoped<IConfigServcie, ConfigService>();
services.AddSingleton<SlpkService>();
services.AddGeoQuery();

var app = builder.Build();

app.UseStaticFiles();
app.MapSlpkEndpoint();
app.UseGeoQuery(builder.Configuration.GetConnectionString("GeoTemplate")!, "api/geo");
app.Run();