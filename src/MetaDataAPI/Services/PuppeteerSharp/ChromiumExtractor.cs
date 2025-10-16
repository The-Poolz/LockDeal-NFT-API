using Mono.Unix;
using System.IO.Compression;
using MetaDataAPI.Services.PuppeteerSharp.Tar;

namespace MetaDataAPI.Services.PuppeteerSharp;

public class ChromiumExtractor
{
    private static readonly string FontConfigEnvVariable = "FONTCONFIG_PATH";
    private static readonly string FontConfigValue = "/tmp";
    private static readonly string LdLibEnvVariable = "LD_LIBRARY_PATH";
    private static readonly string LdLibValue = "/tmp/lib";
    private static readonly string LambdaLayerDirectory = "/opt/chromium-layer";
    public static string ChromiumPath = "/tmp/chromium";

    private static readonly object SyncObject = new();

    private string awsOperatingSystem;
    public string AwsOperatingSystem
    {
        get
        {
            if (awsOperatingSystem != null)
            {
                return awsOperatingSystem;
            }

            if (File.Exists("/etc/system-release-cpe"))
            {
                var osDetails = File
                    .ReadLines("/etc/system-release-cpe")
                    .FirstOrDefault() ?? string.Empty;

                if (osDetails.EndsWith("amazon:amazon_linux:2"))
                {
                    awsOperatingSystem = "al2";
                }
                else if (osDetails.EndsWith("amazon:amazon_linux:2023"))
                {
                    awsOperatingSystem = "al2023";
                }
            }

            return awsOperatingSystem;
        }

        set => awsOperatingSystem = value;
    }

    public string ExtractChromium()
    {
        SetEnvironmentVariables();

        if (File.Exists(ChromiumPath))
        {
            return ChromiumPath;
        }

        lock (SyncObject)
        {
            if (!File.Exists(ChromiumPath))
            {
                if (!string.IsNullOrEmpty(AwsOperatingSystem))
                {
                    ExtractDependencies($"{AwsOperatingSystem}.tar.br", "/tmp");
                }

                ExtractDependencies("fonts.tar.br", "/tmp");
                ExtractDependencies("swiftshader.tar.br", "/tmp");

                var compressedFile = Path.Combine(LambdaLayerDirectory, "chromium.br");

                using var writeFile = File.OpenWrite(ChromiumPath);
                using var readFile = File.OpenRead(compressedFile);
                using (var bs = new BrotliStream(readFile, CompressionMode.Decompress))
                {
                    bs.CopyTo(writeFile);
                }

                _ = new UnixFileInfo(ChromiumPath)
                {
                    FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute | FileAccessPermissions.GroupReadWriteExecute
                };
            }
        }

        return ChromiumPath;
    }

    private static void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("HOME", "/tmp");

        var fontConfig = Environment.GetEnvironmentVariable(FontConfigEnvVariable);
        if (string.IsNullOrEmpty(fontConfig) || !fontConfig.Contains(FontConfigValue))
        {
            var newValue = string.IsNullOrEmpty(fontConfig) ? FontConfigValue : $"{fontConfig}:{FontConfigValue}";
            Environment.SetEnvironmentVariable(FontConfigEnvVariable, newValue);
        }

        var ldLibPath = Environment.GetEnvironmentVariable(LdLibEnvVariable);
        if (string.IsNullOrEmpty(ldLibPath) || !ldLibPath.Contains(LdLibValue))
        {
            var newValue = string.IsNullOrEmpty(ldLibPath) ? LdLibValue : $"{ldLibPath}:{LdLibValue}";
            Environment.SetEnvironmentVariable(LdLibEnvVariable, newValue);
        }
    }

    private static void ExtractDependencies(string fileName, string path)
    {
        var compressedFile = Path.Combine(LambdaLayerDirectory, fileName);

        using var stream = new MemoryStream();
        using var readFile = File.OpenRead(compressedFile);
        using (var bs = new BrotliStream(readFile, CompressionMode.Decompress))
        {
            bs.CopyTo(stream);
            bs.Dispose();
        }

        stream.Seek(0, SeekOrigin.Begin);

        var tarReader = new TarReader(stream);
        tarReader.ReadToEnd(path);
    }
}