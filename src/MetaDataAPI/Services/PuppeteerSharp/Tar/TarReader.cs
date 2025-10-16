namespace MetaDataAPI.Services.PuppeteerSharp.Tar;

/// <summary>
/// Extract contents of a tar file represented by a stream for the TarReader constructor
/// </summary>
public class TarReader
{
    private readonly byte[] dataBuffer = new byte[512];
    private readonly UsTarHeader header;
    private readonly Stream inStream;
    private long remainingBytesInFile;

    /// <summary>
    /// Constructs TarReader object to read data from `tarredData` stream
    /// </summary>
    /// <param name="tarredData">A stream to read tar archive from</param>
    public TarReader(Stream tarredData)
    {
        inStream = tarredData;
        header = new UsTarHeader();
    }

    public ITarHeader FileInfo => header;

    /// <summary>
    /// Read all files from an archive to a directory. It creates some child directories to
    /// reproduce a file structure from the archive.
    /// </summary>
    /// <param name="destDirectory">The out directory.</param>
    /// 
    /// CAUTION! This method is not safe. It's not tar-bomb proof. 
    /// {see http://en.wikipedia.org/wiki/Tar_(file_format) }
    /// If you are not sure about the source of an archive you extracting,
    /// then use MoveNext and Read and handle paths like ".." and "../.." according
    /// to your business logic.
    public void ReadToEnd(string destDirectory)
    {
        while (MoveNext(false))
        {
            var fileNameFromArchive = FileInfo.FileName;
            var totalPath = Path.Join(destDirectory, fileNameFromArchive);
            if (UsTarHeader.IsPathSeparator(fileNameFromArchive[^1]) ||
                FileInfo.EntryType == EntryType.Directory)
            {
                // Record is a directory
                Directory.CreateDirectory(totalPath);
                continue;
            }

            var directory = Path.GetDirectoryName(totalPath);
            Directory.CreateDirectory(directory);
            using var file = File.Create(totalPath);
            Read(file);
        }
    }

    /// <summary>
    /// Read data from a current file to a Stream.
    /// </summary>
    /// <param name="dataDestination">A stream to read data to</param>
    /// 
    /// <seealso cref="MoveNext"/>
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

        var align512 = -1;
        var toRead = remainingBytesInFile - 512;

        if (toRead > 0)
            toRead = 512;
        else
        {
            align512 = 512 - (int)remainingBytesInFile;
            toRead = remainingBytesInFile;
        }

        int bytesRead;
        var bytesRemainingToRead = toRead;
        do
        {

            bytesRead = inStream.Read(dataBuffer, (int)(toRead - bytesRemainingToRead),
                (int)bytesRemainingToRead);
            bytesRemainingToRead -= bytesRead;
            remainingBytesInFile -= bytesRead;
        } while (bytesRead < toRead && bytesRemainingToRead > 0);

        if (inStream.CanSeek && align512 > 0)
        {
            inStream.Seek(align512, SeekOrigin.Current);
        }
        else
            while (align512 > 0)
            {
                inStream.ReadByte();
                --align512;
            }

        buffer = dataBuffer;
        return bytesRead;
    }

    /// <summary>
    /// Check if all bytes in buffer are zeroes
    /// </summary>
    /// <param name="buffer">buffer to check</param>
    /// <returns>true if all bytes are zeroes, otherwise false</returns>
    private static bool IsEmpty(IEnumerable<byte> buffer) => buffer.All(b => b == 0);

    /// <summary>
    /// Move internal pointer to a next file in archive.
    /// </summary>
    /// <param name="skipData">Should be true if you want to read a header only, otherwise false</param>
    /// <returns>false on End Of File otherwise true</returns>
    /// 
    /// Example:
    /// while(MoveNext())
    /// { 
    ///     Read(dataDestStream); 
    /// }
    /// <seealso cref="Read(Stream)"/>
    public bool MoveNext(bool skipData)
    {
        if (remainingBytesInFile > 0)
        {
            if (!skipData)
            {
                throw new TarException("You are trying to change file while not all the data from the previous one was read. If you do want to skip files use skipData parameter set to true.");
            }

            if (inStream.CanSeek)
            {
                var remainer = (remainingBytesInFile % 512);
                inStream.Seek(remainingBytesInFile + (512 - (remainer == 0 ? 512 : remainer)), SeekOrigin.Current);
            }
            else
            {
                byte[] buffer;
                while (Read(out buffer) > 0)
                {
                }
            }
        }

        var bytes = header.GetBytes();
        int headerRead;
        var bytesRemaining = header.HeaderSize;
        do
        {
            headerRead = inStream.Read(bytes, header.HeaderSize - bytesRemaining, bytesRemaining);
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
                headerRead = inStream.Read(bytes, header.HeaderSize - bytesRemaining, bytesRemaining);
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