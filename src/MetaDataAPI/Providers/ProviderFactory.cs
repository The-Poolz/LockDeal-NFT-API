using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Storage;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly IRpcCaller rpcCaller;

    public ProviderFactory(IRpcCaller? rpcCaller = null)
    {
        this.rpcCaller = rpcCaller ?? new RpcCaller();
    }

    public Erc20Token GetErc20Token(string address) => new(address, rpcCaller);

    public bool IsPoolIdWithinSupplyRange(BigInteger poolId) =>
        rpcCaller.GetTotalSupply(Environments.LockDealNftAddress) > poolId;

    public Provider Create(BigInteger poolId) =>
        Create(rpcCaller.GetMetadata(poolId));

    private Provider Create(string metadata) =>
        Create(new BasePoolInfo(metadata,this));

    private static Provider Create(BasePoolInfo basePoolInfo) =>
        Providers(basePoolInfo)[basePoolInfo.ProviderAddress]();

    public static Dictionary<string, Func<Provider>> Providers(BasePoolInfo basePoolInfo) => new()
    {
        { "0x6B31bE09cF4e2da92F130B1056717fEa06176CeD".ToLower(), () => new DealProvider(basePoolInfo) },
        { "0xB9FD557C192939a3889080954D52c64eBA8E9Be3".ToLower(), () => new LockDealProvider(basePoolInfo) },
        { "0x724A076a45ee73544685d4A9Fc2240B1C635711e".ToLower(), () => new TimedDealProvider(basePoolInfo) },
        { "0x70dECfD5e51C59EBdC8AcA96bf22da6aFF00b176".ToLower(), () => new BundleProvider(basePoolInfo) },
        { "0x7254A337D05d3965D7D3d8c1a94Cd1CFCD1b00d6".ToLower(), () => new RefundProvider(basePoolInfo) },
        { "0x8Bf8Cf18c5cB5De075978394624674bA19b96d1B".ToLower(), () => new CollateralProvider(basePoolInfo) }
    };
}