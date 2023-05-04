using System.IO.Compression;

namespace Giserver.Services;

public class ZipService
{
    public async Task<byte[]?> ReadFileAsync(string archiveFileName, string entryName)
    {
        using var archive = ZipFile.OpenRead(archiveFileName);
        var entry = archive.GetEntry(entryName);

        if (entry == null) return null;

        using var stream = entry.Open();
        var buffer = new byte[stream.Length];
        await stream.ReadAsync(buffer, 0, buffer.Length);

        return buffer;
    }
}