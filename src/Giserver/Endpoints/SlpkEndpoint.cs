namespace Giserver.Endpoints;

public static class SlpkEndpoint
{
    public static WebApplication UseSlpkEndpoints(this WebApplication app)
    {
        var prefix = "api/slpk";
        var group = app.MapGroup(prefix + "/{resourceId}/SceneServer/layers/0")!;
        group.WithTags("slpk");

        group.MapGet("/", async (HttpContext context, SlpkService slpkService, string resourceId, string? relativePath) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, relativePath ?? "" + "3dSceneLayer.json");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/nodepages/{nodePageID}", async (HttpContext context, SlpkService slpkService, string resourceId, string nodePageID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"nodepages/{nodePageID}.json");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/nodes/{nodeID}/textures/{textureID}", async (HttpContext context, SlpkService slpkService, string resourceId, string nodeID, string textureID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/textures/{textureID}.bin.dds");
            if (buffer != null)
            {
                context.Response.Headers.Append("Content-Encoding", "gzip");
                return Results.Bytes(buffer, "application/octet-stream;charset=binary");
            }

            buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/textures/{textureID}.ktx2");
            if (buffer != null)
            {
                context.Response.Headers.Append("Content-Encoding", "gzip");
                return Results.Bytes(buffer, "application/octet-stream;charset=binary");
            }

            buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/textures/{textureID}.jpg")
                ?? await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/textures/{textureID}.png")
                ?? await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/textures/{textureID}.bin");

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return buffer != null ? Results.Bytes(buffer, "image/jpeg") : Results.NotFound();
        });

        group.MapGet("/nodes/{nodeID}/geometries/{geometryID}", async (HttpContext context, SlpkService slpkService, string resourceId, string nodeID, string geometryID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/geometries/{geometryID}.bin");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");

            return Results.Bytes(buffer, "application/octet-stream; charset=binary");
        });

        group.MapGet("/nodes/{nodeID}/attributes/f_{attributeID}/0", async (HttpContext context, SlpkService slpkService, string resourceId, string nodeID, string attributeID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/attributes/f_{attributeID}/0.bin");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/octet-stream; charset=binary");
        });

        group.MapGet("/nodes/{nodeID}/shared", async (HttpContext context, SlpkService slpkService, string resourceId, string nodeID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/shared/sharedResource.json");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/nodes/{nodeID}", async (HttpContext context, SlpkService slpkService, string resourceId, string nodeID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"nodes/{nodeID}/3dNodeIndexDocument.json");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/statistics/f_{attributeID}/0", async (HttpContext context, SlpkService slpkService, string resourceId, string attributeID) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, $"statistics/f_{attributeID}/0.json");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/statistics/summary", async (HttpContext context, SlpkService slpkService, string resourceId) =>
        {
            var buffer = await slpkService.ReadFileAsync(resourceId, "/statistics/summary.json");
            if (buffer == null) return Results.NotFound();

            context.Response.Headers.Append("Content-Encoding", "gzip");
            return Results.Bytes(buffer, "application/json");
        });

        group.MapGet("/sublayers/{sublayerID}/{**lps}", (HttpContext context, string resourceId, string sublayerID, string? lps) =>
        {
            var domain = context.Request.Scheme + "://" + context.Request.Host.Value;
            var url = lps == null ?
                $"{domain}/{prefix}/{resourceId}/SceneServer/layers/0?relativePath=sublayers/{sublayerID}" :
                $"{domain}/{prefix}/{resourceId}/3DObjectSceneLayer/SceneServer/layers/0/{lps}?relativePath=sublayers/{sublayerID}";

            return Results.RedirectToRoute(url);
        });

        return app;
    }
}