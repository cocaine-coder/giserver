namespace Giserver.Services;

public interface IConfigServcie
{
    Task<(string? path, bool dir)> GetSlpkPathAsync(string resourceId);
}

public class ConfigService : IConfigServcie
{
    public Task<(string? path, bool dir)> GetSlpkPathAsync(string resourceId)
    {
        //return Task.FromResult((@"G:\demo\i3s-spec\slpks\integrated", true));
        return Task.FromResult((@"G:\demo\TEMP", true));
    }
}