using Giserver.Endpoints.Vector.Dto;

namespace Giserver.Endpoints.Vector;

public class UpdateFeatureGeometryRequest : VectorUrlValues
{
    public Geometry Geometry { get; set; }
}

public class UpdateFeatureGeometry : Endpoint<UpdateFeatureGeometryRequest>
{
    public override void Configure()
    {
        Put("vector/{database}/{table}");
    }

    public override Task HandleAsync(UpdateFeatureGeometryRequest req, CancellationToken ct)
    {
        return base.HandleAsync(req, ct);
    }
}