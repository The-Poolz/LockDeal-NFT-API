using Mono.Unix;
using System.IO.Compression;
using MetaDataAPI.Services.PuppeteerSharp.Tar;

namespace MetaDataAPI.Services.PuppeteerSharp;

public class ChromiumExtractor
{
    private const string FontConfigEnvVariable = "FONTCONFIG_PATH";
    private const string FontConfigValue = "/tmp";
    private const string LdLibEnvVariable = "LD_LIBRARY_PATH";
    private const string LdLibValue = "/tmp/lib";
    private const string LambdaLayerDirectory = "/opt/chromium-layer";
    private const string ChromiumPath = "/tmp/chromium";

    private static readonly object SyncObject = new();

    private string? awsOperatingSystem;
    public string? AwsOperatingSystem
    {
        get => awsOperatingSystem ??= DetectAwsOperatingSystem();
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

                using var readFile = File.OpenRead(compressedFile);
                using var writeFile = File.Create(ChromiumPath);
                using var brotliStream = new BrotliStream(readFile, CompressionMode.Decompress);
                brotliStream.CopyTo(writeFile);

                _ = new UnixFileInfo(ChromiumPath)
                {
                    FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute | FileAccessPermissions.GroupReadWriteExecute,
                };
            }
        }

        return ChromiumPath;
    }

    private static string? DetectAwsOperatingSystem()
    {
        const string systemReleaseFile = "/etc/system-release-cpe";

        if (!File.Exists(systemReleaseFile))
        {
            return null;
        }

        var osDetails = File.ReadLines(systemReleaseFile).FirstOrDefault();
        if (string.IsNullOrEmpty(osDetails))
        {
            return null;
        }

        if (osDetails.EndsWith("amazon:amazon_linux:2"))
        {
            return "al2";
        }

        return osDetails.EndsWith("amazon:amazon_linux:2023") ? "al2023" : null;
    }

    private static void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("HOME", "/tmp");
        EnsureEnvironmentContains(FontConfigEnvVariable, FontConfigValue);
        EnsureEnvironmentContains(LdLibEnvVariable, LdLibValue);
    }

    private static void EnsureEnvironmentContains(string variableName, string requiredValue)
    {
        var currentValue = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrEmpty(currentValue))
        {
            Environment.SetEnvironmentVariable(variableName, requiredValue);
            return;
        }

        if (!currentValue.Split(':', StringSplitOptions.RemoveEmptyEntries).Contains(requiredValue, StringComparer.Ordinal))
        {
            Environment.SetEnvironmentVariable(variableName, $"{currentValue}:{requiredValue}");
        }
    }

    private static void ExtractDependencies(string fileName, string destinationPath)
    {
        var compressedFile = Path.Combine(LambdaLayerDirectory, fileName);

        using var readFile = File.OpenRead(compressedFile);
        using var decompressedStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(readFile, CompressionMode.Decompress))
        {
            brotliStream.CopyTo(decompressedStream);
        }

        decompressedStream.Position = 0;
        new TarReader(decompressedStream).ReadToEnd(destinationPath);
    }
}