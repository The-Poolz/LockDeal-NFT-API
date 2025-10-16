﻿using Microsoft.Extensions.Logging;
using System.Formats.Tar;
using System.IO.Compression;
using Mono.Unix;

namespace MetaDataAPI.Services.PuppeteerSharp;

public class ChromiumExtractor
{
    private static string FontConfigEnvVariable = "FONTCONFIG_PATH";
    private static string FontConfigValue = "/tmp";
    private static string LdLibEnvVariable = "LD_LIBRARY_PATH";
    private static string LdLibValue = "/tmp/lib";

    public static string ChromiumPath = "/tmp/chromium";

    private static readonly object SyncObject = new object();
    private readonly ILogger<ChromiumExtractor> logger;
    private readonly ILoggerFactory loggerFactory;

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

    public ChromiumExtractor(ILoggerFactory loggerFactory)
    {
        this.loggerFactory = loggerFactory;
        logger = loggerFactory.CreateLogger<ChromiumExtractor>();
    }

    /// <summary>
    /// Extracts chromium to temp path, if not already completed
    /// </summary>
    /// <returns>Path to chromium bin</returns>
    public string ExtractChromium()
    {
        SetEnvironmentVariables();

        if (!Directory.Exists("/tmp"))
        {
            logger.LogDebug("/tmp doesn't exist.  Is this running on lambda?");
        }

        // Quick bale if exec exists
        if (File.Exists(ChromiumPath))
        {
            return ChromiumPath;
        }

        logger.LogDebug("Chromium doesn't exist, extracting");

        lock (SyncObject)
        {
            if (!File.Exists(ChromiumPath))
            {
                if (!string.IsNullOrEmpty(AwsOperatingSystem))
                {
                    ExtractDependencies($"{AwsOperatingSystem}.tar.br", "/tmp");
                }
                else
                {
                    logger.LogWarning("Operating environment unexpected. Unable to extract correct dependencies.");
                }

                ExtractDependencies("fonts.tar.br", "/tmp");
                ExtractDependencies("swiftshader.tar.br", "/tmp");

                var compressedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chromium.br");

                logger.LogDebug($"Found compressed file {compressedFile}");

                using (var writeFile = File.OpenWrite(ChromiumPath))
                using (var readFile = File.OpenRead(compressedFile))
                {
                    logger.LogDebug($"Extracting chromium to {ChromiumPath}");

                    using (var bs = new BrotliStream(readFile, CompressionMode.Decompress))
                    {
                        bs.CopyTo(writeFile);
                        bs.Dispose();
                    }
                }

                logger.LogInformation("Extracted chromium to {ChromiumPath}", ChromiumPath);
            }
        }

        return ChromiumPath;
    }

    private void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("HOME", "/tmp");

        var fontConfig = Environment.GetEnvironmentVariable(FontConfigEnvVariable);
        if (string.IsNullOrEmpty(fontConfig) || !fontConfig.Contains(FontConfigValue))
        {
            var newValue = string.IsNullOrEmpty(fontConfig) ? FontConfigValue : $"{fontConfig}:{FontConfigValue}";
            logger.LogDebug("Setting {FontConfigEnvVariable} to {FontConfigValue}", FontConfigEnvVariable, newValue);
            Environment.SetEnvironmentVariable(FontConfigEnvVariable, newValue);
        }

        var ldLibPath = Environment.GetEnvironmentVariable(LdLibEnvVariable);
        if (string.IsNullOrEmpty(ldLibPath) || !ldLibPath.Contains(LdLibValue))
        {
            var newValue = string.IsNullOrEmpty(ldLibPath) ? LdLibValue : $"{ldLibPath}:{LdLibValue}";
            logger.LogDebug("Setting {LdLibEnvVariable} to {LdLibValue} ", LdLibEnvVariable, newValue);
            Environment.SetEnvironmentVariable(LdLibEnvVariable, newValue);
        }
    }

    private void ExtractDependencies(string fileName, string path)
    {
        var compressedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        logger.LogDebug($"Found compressed file {compressedFile}");
        using (var stream = new MemoryStream())
        using (var readFile = File.OpenRead(compressedFile))
        {
            using (var bs = new BrotliStream(readFile, CompressionMode.Decompress))
            {
                bs.CopyTo(stream);
                bs.Dispose();
            }

            stream.Seek(0, SeekOrigin.Begin);

            var tarReader = new TarReader(stream, loggerFactory);
            tarReader.ReadToEnd(path);
        }
    }
}