using Giserver.Endpoints.Vector.Dto;

namespace Giserver.Endpoints.Vector;

public class UpdateFeaturePropertiesRequest : VectorUrlValues
{
}

public class UpdateFeatureProperties : Endpoint<UpdateFeaturePropertiesRequest>
{
    public override void Configure()
    {
        Patch("vector/{database}/{table}");
    }

    public override Task HandleAsync(UpdateFeaturePropertiesRequest req, CancellationToken ct)
    {
        return base.HandleAsync(req, ct);
    }
}