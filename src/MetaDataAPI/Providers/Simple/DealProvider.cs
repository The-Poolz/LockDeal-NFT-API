using MetaDataAPI.Models.Response;
namespace MetaDataAPI.Providers;

public class DealProvider : Provider
{
    public override string ToString() => $"immediate access to {LeftAmount}"; 
    public override string ProviderName => nameof(DealProvider);
    public override string Description =>
        $"This NFT represents immediate access to {LeftAmount} units of the specified asset {PoolInfo.Token}.";

    public DealProvider(BasePoolInfo basePoolInfo)
        : base(basePoolInfo) { }
}