namespace Giserver.Services;

public class SlpkService
{
    private readonly Dictionary<string, SlpkConfig> _slpkConfig;

    public SlpkService(IOptions<Dictionary<string, SlpkConfig>> options)
    {
        this._slpkConfig = options.Value;
    }

    public async Task<byte[]?> ReadFileAsync(string resourceId, string relativePath, CancellationToken cancellationToken = default)
    {
        if (!_slpkConfig.TryGetValue(resourceId, out var result))
            return null;

        var (path, dir) = result;

        if (dir)
        {
            var filePath = Path.Combine(path, relativePath);

            if (File.Exists(filePath))
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                using var compressedStream = new MemoryStream();
                var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, true);
                await stream.CopyToAsync(gzipStream, cancellationToken);
                await gzipStream.DisposeAsync();

                return compressedStream.ToArray();
            }

            if (File.Exists(filePath + ".gz"))
                return await File.ReadAllBytesAsync(filePath + ".gz", cancellationToken);

            return null;
        }
        else
        {
            using var archive = ZipFile.OpenRead(path);
            var entry = archive.GetEntry(relativePath);

            if (entry != null)
            {
                using var stream = entry.Open();
                using var compressedStream = new MemoryStream();
                var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, true);
                await stream.CopyToAsync(gzipStream, cancellationToken);
                await gzipStream.DisposeAsync();

                return compressedStream.ToArray();
            }

            entry = archive.GetEntry(relativePath + ".gz");
            if (entry != null)
            {
                using var stream = entry.Open();
                var buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                return buffer;
            }

            return null;
        }
    }
}