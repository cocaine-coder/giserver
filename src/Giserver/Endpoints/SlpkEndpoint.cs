using Giserver.Services;

namespace Giserver.Endpoints;

public static class SlpkEndpoint
{
    public static WebApplication MapSlpkEndpoint(this WebApplication app, string? prefix = default)
    {
        prefix = prefix ?? "" + "/slpk";

        var group = app.MapGroup(prefix + "/{resourceId}/SceneServer/layers/0");

        group.MapGet("/", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string? relativePath) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, relativePath ?? "" + "3dSceneLayer.json.gz");
            if (buffer == null) return Results.NotFound($"scene layer document read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        }).WithTags("3DSceneLayer");

        group.MapGet("/nodepages/{nodePageID}", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string nodePageID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"nodepages/{nodePageID}.json.gz");
            if (buffer == null) return Results.NotFound($"node page : ({nodePageID}) read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        }).WithTags("nodePage");

        group.MapGet("/nodes/{nodeID}/textures/{textureID}", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string nodeID, string textureID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.bin.dds.gz");
            if (buffer != null)
            {
                context.Response.Headers.Add("Content-Encoding", "gzip");
                return Results.Bytes(buffer, "application/octet-stream; charset=binary");
            }
            else
            {
                buffer = await zipService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.jpg")
                    ?? await zipService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.png")
                    ?? await zipService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.bin");

                if (buffer != null)
                {
                    return Results.Bytes(buffer, "image/jpeg");
                }
                else
                {
                    return Results.NotFound();
                }
            }
        });

        group.MapGet("/nodes/{nodeID}/geometries/{geometryID}", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string nodeID, string geometryID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"nodes/{nodeID}/geometries/{geometryID}.bin.gz");
            if (buffer == null) return Results.NotFound($"geometry : ({geometryID}) read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");

            return Results.Bytes(buffer, "application/octet-stream; charset=binary");
        });

        group.MapGet("/nodes/{nodeID}/attributes/f_{attributeID}/0", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string nodeID, string attributeID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"nodes/{nodeID}/attributes/f_{attributeID}/0.bin.gz");
            if (buffer == null) return Results.NotFound($"attribute : ({attributeID}) read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/octet-stream; charset=binary");
        });

        group.MapGet("/nodes/{nodeID}/shared", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string nodeID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"nodes/{nodeID}/shared/sharedResource.json.gz");
            if (buffer == null) return Results.NotFound($"shared : nodeId ({nodeID}) read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/nodes/{nodeID}", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string nodeID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"nodes/{nodeID}/3dNodeIndexDocument.json.gz");
            if (buffer == null) return Results.NotFound($"3D node index document : nodeId ({nodeID}) read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/statistics/f_{attributeID}/0", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId, string attributeID) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, $"statistics/f_{attributeID}/0.json.gz");
            if (buffer == null) return Results.NotFound($"statistics : ({attributeID}) read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/statistics/summary", async (HttpContext context, IConfiguration configuration, ZipService zipService, string resourceId) =>
        {
            var path = configuration.GetValue<string>($"Slpk:{resourceId}:Path");
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await zipService.ReadFileAsync(path, "/statistics/summary.json.gz");
            if (buffer == null) return Results.NotFound($"statistics summary document read error");

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/sublayers/{sublayerID}", (HttpContext context, string resourceId, string sublayerID) =>
        {
            var domain = context.Request.Scheme + "://" + context.Request.Host.Value;
            var url = $"{domain}/{prefix}/{resourceId}/SceneServer/layers/0?relativePath=sublayers/{sublayerID}";
            return Results.RedirectToRoute(url);
        });

        group.MapGet("/sublayers/{sublayerID}/{**lps}", (HttpContext context, string resourceId, string sublayerID, string lps) =>
        {
            var domain = context.Request.Scheme + "://" + context.Request.Host.Value;
            var url = $"{domain}/{prefix}/{resourceId}/3DObjectSceneLayer/SceneServer/layers/0/{lps}?relativePath=sublayers/{sublayerID}";
            return Results.Redirect(url);
        });

        return app;
    }

    private static WebApplication MapPointEndpoint(this WebApplication app, string prefix)
    {
        return app;
    }

    private static WebApplication MapPCSLEndpoint(this WebApplication app, string prefix)
    {
        return app;
    }

}