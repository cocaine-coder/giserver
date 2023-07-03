namespace Giserver.Endpoints.Vector.Dto;

public class VectorUrlValues
{
    [FromRoute(Name = "database")]
    public string Database { get; set; }

    [FromRoute(Name = "table")]
    public string Table { get; set; }

    [FromQueryParams]
    public string? Schema { get; set; }
}