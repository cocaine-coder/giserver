using Giserver.Services;

namespace Giserver.Endpoints;

public static class SlpkEndpoint
{
    public static WebApplication MapSlpkEndpoint(this WebApplication app, string? prefix = default)
    {
        prefix = prefix ?? "" + "/slpk";

        var group = app.MapGroup(prefix + "/{resourceId}/SceneServer/layers/0")!;

        group.MapGet("/", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string? relativePath) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound($"resource id : {resourceId} not found");

            var buffer = await slpkService.ReadFileAsync(path, relativePath ?? "" + "3dSceneLayer.json", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        }).WithTags("3DSceneLayer");

        group.MapGet("/nodepages/{nodePageID}", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string nodePageID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"nodepages/{nodePageID}.json", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        }).WithTags("nodePage");

        group.MapGet("/nodes/{nodeID}/textures/{textureID}", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string nodeID, string textureID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.bin.dds", dir);
            if (buffer != null)
            {
                context.Response.Headers.Add("Content-Encoding", "gzip");
                return Results.Bytes(buffer, "application/octet-stream;charset=binary");
            }

            buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.ktx2", dir);
            if (buffer != null)
            {
                context.Response.Headers.Add("Content-Encoding", "gzip");
                return Results.Bytes(buffer, "application/octet-stream;charset=binary");
            }

            buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.jpg", dir)
                ?? await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.png", dir)
                ?? await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/textures/{textureID}.bin", dir);

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return buffer != null ? Results.Bytes(buffer, "image/jpeg") : Results.NotFound();

        });

        group.MapGet("/nodes/{nodeID}/geometries/{geometryID}", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string nodeID, string geometryID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/geometries/{geometryID}.bin", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");

            return Results.Bytes(buffer, "application/octet-stream; charset=binary");
        });

        group.MapGet("/nodes/{nodeID}/attributes/f_{attributeID}/0", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string nodeID, string attributeID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/attributes/f_{attributeID}/0.bin", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/octet-stream; charset=binary");
        });

        group.MapGet("/nodes/{nodeID}/shared", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string nodeID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/shared/sharedResource.json", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/nodes/{nodeID}", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string nodeID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"nodes/{nodeID}/3dNodeIndexDocument.json", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/statistics/f_{attributeID}/0", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId, string attributeID) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, $"statistics/f_{attributeID}/0.json", dir);
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Add("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/statistics/summary", async (HttpContext context, IConfigServcie configServcie, SlpkService slpkService, string resourceId) =>
        {
            var (path, dir) = await configServcie.GetSlpkPathAsync(resourceId);
            if (string.IsNullOrEmpty(path)) return Results.NotFound();

            var buffer = await slpkService.ReadFileAsync(path, "/statistics/summary.json", dir);
            if (buffer == null) return Results.NotFound();

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