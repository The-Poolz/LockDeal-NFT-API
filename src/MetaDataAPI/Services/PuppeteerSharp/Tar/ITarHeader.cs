namespace MetaDataAPI.Services.PuppeteerSharp.Tar;

public interface ITarHeader
{
    public string FileName { get; set; }
    public EntryType EntryType { get; set; }
}