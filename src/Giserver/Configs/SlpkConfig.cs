namespace Giserver.Configs;

public class SlpkConfig
{
    public string Path { get; set; }

    public bool IsDir { get; set; }

    public bool Exists => IsDir ?
        Directory.Exists(Path) :
        File.Exists(Path);

    public void Deconstruct(out string path, out bool isDir)
    {
        path = Path;
        isDir = IsDir;
    }
}