using Giserver.Endpoints.Vector.Dto;

namespace Giserver.Endpoints.Vector;

public class DeleteFeatureRequest : VectorUrlValues
{
}

public class DeleteFeature : Endpoint<DeleteFeatureRequest>
{
    public override void Configure()
    {
        Delete("vector/{database}/{table}");
    }

    public override Task HandleAsync(DeleteFeatureRequest req, CancellationToken ct)
    {
        return base.HandleAsync(req, ct);
    }
}