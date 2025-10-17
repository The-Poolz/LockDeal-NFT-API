using Mono.Unix;
using System.Formats.Tar;
using System.IO.Compression;

namespace MetaDataAPI.Services.PuppeteerSharp;

public sealed class ChromiumExtractor
{
    private const string LayerDir = "/opt/chromium-layer";
    private const string TmpDir = "/tmp";
    private const string ChromiumPath = $"{TmpDir}/chromium";

    private static readonly object Gate = new();

    public static string ExtractChromium()
    {
        SetEnv();

        if (File.Exists(ChromiumPath)) return ChromiumPath;

        lock (Gate)
        {
            if (File.Exists(ChromiumPath)) return ChromiumPath;

            foreach (var pack in GetPacks())
            {
                ExtractTarBr(Path.Combine(LayerDir, pack), TmpDir);
            }

            using var src = File.OpenRead(Path.Combine(LayerDir, "chromium.br"));
            using var br = new BrotliStream(src, CompressionMode.Decompress);
            using var dst = File.Create(ChromiumPath);
            br.CopyTo(dst);

            _ = new UnixFileInfo(ChromiumPath)
            {
                FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute | FileAccessPermissions.GroupReadWriteExecute
            };

            return ChromiumPath;
        }
    }

    private static IEnumerable<string> GetPacks()
    {
        var os = DetectAwsOs();
        if (!string.IsNullOrEmpty(os)) yield return $"{os}.tar.br";
        yield return "fonts.tar.br";
        yield return "swiftshader.tar.br";
    }

    private static void ExtractTarBr(string src, string dest)
    {
        using var fs = File.OpenRead(src);
        using var br = new BrotliStream(fs, CompressionMode.Decompress);
        TarFile.ExtractToDirectory(br, dest, overwriteFiles: true);
    }

    private static string? DetectAwsOs()
    {
        const string cpe = "/etc/system-release-cpe";
        if (!File.Exists(cpe)) return null;

        var line = File.ReadLines(cpe).FirstOrDefault() ?? string.Empty;
        if (line.EndsWith("amazon:amazon_linux:2")) return "al2";
        if (line.EndsWith("amazon:amazon_linux:2023")) return "al2023";
        return null;
    }

    private static void SetEnv()
    {
        Environment.SetEnvironmentVariable("HOME", TmpDir);
        AppendEnv("FONTCONFIG_PATH", $"{TmpDir}");
        AppendEnv("LD_LIBRARY_PATH", $"{TmpDir}/lib");
    }

    private static void AppendEnv(string key, string value)
    {
        var cur = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(cur))
        {
            Environment.SetEnvironmentVariable(key, value);
            return;
        }

        if (!cur.Split(':', StringSplitOptions.RemoveEmptyEntries).Contains(value, StringComparer.Ordinal))
        {
            Environment.SetEnvironmentVariable(key, $"{cur}:{value}");
        }
    }
}