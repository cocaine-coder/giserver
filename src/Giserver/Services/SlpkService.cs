using System.IO.Compression;
using Giserver.Extensions;

namespace Giserver.Services;

public class SlpkService
{

    public SlpkService()
    {
    }

    public async Task<byte[]?> ReadFileAsync(string path, string relativePath, bool dir)
    {
        if (dir)
        {
            var filePath = Path.Combine(path, relativePath);
            if (File.Exists(filePath))
            {
                var buffer = await File.ReadAllBytesAsync(filePath);
                using var compressedStream = new MemoryStream();
                var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, true);
                gzipStream.Write(buffer, 0, buffer.Length);
                await gzipStream.DisposeAsync();
                return compressedStream.ToArray();
            }

            else if (File.Exists(filePath + ".gz"))
                return await File.ReadAllBytesAsync(filePath + ".gz");
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
                await stream.CopyToAsync(gzipStream);
                await gzipStream.DisposeAsync();
                return compressedStream.ToArray();
            }
            else
            {
                entry = archive.GetEntry(relativePath + ".gz");
                if (entry == null) return null;
                else
                {
                    using var stream = entry.Open();
                    return await stream.ToLitBufferAsync();
                }
            }
        }
    }
}