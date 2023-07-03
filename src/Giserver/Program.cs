var builder = WebApplication.CreateSlimBuilder(args);
var isDev = builder.Environment.IsDevelopment();
var services = builder.Services;
var configuration = builder.Configuration;
configuration.AddJsonFile($"appsetting.{(isDev ? "dev." : "")}json");

services.AddGeoQuery();
services.AddSingleton<SlpkService>();
services.AddFastEndpoints();

services.AddOptions<Dictionary<string, SlpkConfig>>("Slpk");

if (isDev)
{
    services.SwaggerDocument(o =>
    {
        o.TagCase = TagCase.LowerCase;
        o.ShortSchemaNames = true;
        o.SerializerSettings = settings =>
        {
            settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        };
        o.DocumentSettings = settings =>
        {
            settings.TypeMappers.AddGeometry(GeoSerializeType.Geojson);
        };
    });
}

var app = builder.Build();

app.UseStaticFiles();

app.UseSlpkEndpoints();
app.UseGeoQuery(configuration.GetConnectionString("GeoTemplate")!, "api/vector", routeHandlerBuilder =>
{
    routeHandlerBuilder.WithTags("vector");
});
app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Serializer.Options.Converters.Add(new GeoJsonConverterFactory());
});

if (isDev)
{
    app.UseSwaggerGen();
}
app.Run();