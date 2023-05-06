namespace Giserver.Extensions;

public static class StreamExtensions
{
    public static async Task<byte[]> ToLitBufferAsync(this Stream stream)
    {
        var buffer = new byte[stream.Length];
        await stream.ReadAsync(buffer, 0, buffer.Length);
        return buffer;
    }
}