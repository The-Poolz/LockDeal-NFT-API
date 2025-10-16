namespace MetaDataAPI.Services.PuppeteerSharp.Tar;

public class TarReader(Stream tarredData)
{
    private readonly byte[] dataBuffer = new byte[512];
    private readonly UsTarHeader header = new();
    private long remainingBytesInFile;

    public ITarHeader FileInfo => header;

    public void ReadToEnd(string destDirectory)
    {
        while (MoveNext(false))
        {
            var fileNameFromArchive = FileInfo.FileName;
            var totalPath = Path.Join(destDirectory, fileNameFromArchive);
            if (UsTarHeader.IsPathSeparator(fileNameFromArchive[^1]) ||
                FileInfo.EntryType == EntryType.Directory)
            {
                Directory.CreateDirectory(totalPath);
                continue;
            }

            var directory = Path.GetDirectoryName(totalPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using var file = File.Create(totalPath);
            Read(file);
        }
    }

    public void Read(Stream dataDestination)
    {
        int readBytes;
        while ((readBytes = Read(out var read)) != -1)
        {
            dataDestination.Write(read, 0, readBytes);
        }
    }

    protected int Read(out byte[] buffer)
    {
        if (remainingBytesInFile == 0)
        {
            buffer = null;
            return -1;
        }

        var bytesRemainingBeforeRead = remainingBytesInFile;
        var toRead = (int)Math.Min(bytesRemainingBeforeRead, dataBuffer.Length);
        var totalBytesRead = 0;
        while (totalBytesRead < toRead)
        {
            var read = tarredData.Read(dataBuffer, totalBytesRead, toRead - totalBytesRead);
            if (read == 0)
            {
                break;
            }

            totalBytesRead += read;
            remainingBytesInFile -= read;
        }

        if (toRead > 0 && totalBytesRead == 0)
        {
            throw new TarException("Unexpected end of tar stream");
        }

        var paddingBytes = bytesRemainingBeforeRead % 512 == 0
            ? 0
            : (int)(512 - bytesRemainingBeforeRead % 512);

        if (remainingBytesInFile == 0 && paddingBytes > 0)
        {
            if (tarredData.CanSeek)
            {
                tarredData.Seek(paddingBytes, SeekOrigin.Current);
            }
            else
            {
                for (var i = 0; i < paddingBytes; i++)
                {
                    tarredData.ReadByte();
                }
            }
        }

        buffer = dataBuffer;
        return totalBytesRead == 0 ? -1 : totalBytesRead;
    }

    private static bool IsEmpty(IEnumerable<byte> buffer) => buffer.All(b => b == 0);

    public bool MoveNext(bool skipData)
    {
        if (remainingBytesInFile > 0)
        {
            if (!skipData)
            {
                throw new TarException("You are trying to change file while not all the data from the previous one was read. If you do want to skip files use skipData parameter set to true.");
            }

            if (tarredData.CanSeek)
            {
                var remainer = remainingBytesInFile % 512;
                tarredData.Seek(remainingBytesInFile + (512 - (remainer == 0 ? 512 : remainer)), SeekOrigin.Current);
            }
            else
            {
                while (Read(out _) > 0) { }
            }
        }

        var bytes = header.GetBytes();
        int headerRead;
        var bytesRemaining = header.HeaderSize;
        do
        {
            headerRead = tarredData.Read(bytes, header.HeaderSize - bytesRemaining, bytesRemaining);
            bytesRemaining -= headerRead;
            if (headerRead <= 0 && bytesRemaining > 0)
            {
                throw new TarException("Can not read header");
            }
        } while (bytesRemaining > 0);

        if (IsEmpty(bytes))
        {
            bytesRemaining = header.HeaderSize;
            do
            {
                headerRead = tarredData.Read(bytes, header.HeaderSize - bytesRemaining, bytesRemaining);
                bytesRemaining -= headerRead;
                if (headerRead <= 0 && bytesRemaining > 0)
                {
                    throw new TarException("Broken archive");
                }

            } while (bytesRemaining > 0);

            if (bytesRemaining == 0 && IsEmpty(bytes))
            {
                return false;
            }

            throw new TarException("Broken archive");
        }

        if (header.UpdateHeaderFromBytes())
        {
            throw new TarException("Checksum check failed");
        }

        remainingBytesInFile = header.SizeInBytes;

        return true;
    }
}