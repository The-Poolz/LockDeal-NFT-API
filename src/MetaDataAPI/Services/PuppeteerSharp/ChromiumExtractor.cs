using System.Formats.Tar;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace MetaDataAPI.Services.PuppeteerSharp;

public static class ChromiumExtractor
{
    private const string LayerDir = "/opt/chromium-layer";
    private const string TmpDir = "/tmp";
    private const string ChromiumPath = $"{TmpDir}/chromium";

    private static readonly Lazy<string> LazyPath = new(Init, LazyThreadSafetyMode.ExecutionAndPublication);

    public static string ExtractChromium() => LazyPath.Value;

    private static string Init()
    {
        SetEnv();

        if (!File.Exists(ChromiumPath))
        {
            foreach (var pack in GetPacks())
                ExtractTarBr($"{LayerDir}/{pack}", TmpDir);

            using var src = File.OpenRead($"{LayerDir}/chromium.br");
            using var br = new BrotliStream(src, CompressionMode.Decompress);
            using var dst = File.Create(ChromiumPath);
            br.CopyTo(dst);

            if (OperatingSystem.IsLinux() && RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                File.SetUnixFileMode(
                    ChromiumPath,
                    UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                    UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute
                );
            }
        }

        return ChromiumPath;
    }

    private static IEnumerable<string> GetPacks()
    {
        const string cpe = "/etc/system-release-cpe";
        var line = File.Exists(cpe) ? File.ReadLines(cpe).FirstOrDefault() : null;
        if (line?.EndsWith("amazon:amazon_linux:2") == true) yield return "al2.tar.br";
        else if (line?.EndsWith("amazon:amazon_linux:2023") == true) yield return "al2023.tar.br";
        yield return "fonts.tar.br";
        yield return "swiftshader.tar.br";
    }

    private static void ExtractTarBr(string src, string dest)
    {
        using var br = new BrotliStream(File.OpenRead(src), CompressionMode.Decompress);
        TarFile.ExtractToDirectory(br, dest, overwriteFiles: true);
    }

    private static void SetEnv()
    {
        Environment.SetEnvironmentVariable("HOME", TmpDir);
        AppendEnv("FONTCONFIG_PATH", TmpDir);
        AppendEnv("LD_LIBRARY_PATH", $"{TmpDir}/lib");
    }

    private static void AppendEnv(string key, string value)
    {
        var cur = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(cur))
            Environment.SetEnvironmentVariable(key, value);
        else if (!cur.Split(':', StringSplitOptions.RemoveEmptyEntries).Contains(value, StringComparer.Ordinal))
            Environment.SetEnvironmentVariable(key, $"{cur}:{value}");
    }
}