using Giserver.Endpoints.Vector.Dto;

namespace Giserver.Endpoints.Vector;

public class CreateFeatureRequest : VectorUrlValues
{
    public DbColumnValue<Geometry> Geometry { get; set; }

    public Dictionary<string, JSTypeValue> Properties { get; set; }
}

public class CreateFeature : Endpoint<CreateFeatureRequest>
{
    public override void Configure()
    {
        Post("vector/{database}/{table}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateFeatureRequest req, CancellationToken ct)
    {
        await SendAsync(req);
    }
}