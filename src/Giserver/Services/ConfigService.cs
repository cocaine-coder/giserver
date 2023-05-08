namespace Giserver.Services;

public interface IConfigServcie
{
    Task<(string? path, bool dir)> GetSlpkPathAsync(string resourceId);
}

public class ConfigService : IConfigServcie
{
    public Task<(string? path, bool dir)> GetSlpkPathAsync(string resourceId)
    {
        //return Task.FromResult((@"Z:\temp\test.slpk", false));
        return Task.FromResult((@"Z:\temp\test", true));
    }
}