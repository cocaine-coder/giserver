namespace Giserver.Services;

public class ArrayBufferPool : IDisposable
{
    private static ArrayPool<byte> _arrayPool = ArrayPool<byte>.Create();

    private byte[]? _array { get; set; } = null;

    public byte[] Rent(int count)
    {
        // 防止二次租用
        if (_array != null)
            _arrayPool.Return(_array);

        _array = ArrayBufferPool._arrayPool.Rent(count);

        return _array;
    }

    public void Dispose()
    {
        // 请求结束容器自动执行
        if (_array != null)
            ArrayBufferPool._arrayPool.Return(_array);
    }
}