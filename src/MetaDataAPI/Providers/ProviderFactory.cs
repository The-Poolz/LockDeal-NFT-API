using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public class ProviderFactory
{
    private readonly IRpcCaller RpcCaller;
    public ProviderFactory(IRpcCaller? rpcCaller = null)
    {
        RpcCaller = rpcCaller ?? new RpcCaller();
    }
    public IProvider FromPoolId(BigInteger poolId) => FromMetdata(RpcCaller.GetMetadata(poolId));
    public IProvider FromMetdata(string metadata) => FromPoolInfo(new BasePoolInfo(metadata,this));
    public IProvider FromPoolInfo(BasePoolInfo basePoolInfo) => Create(basePoolInfo)!;
    internal IProvider? Create(BasePoolInfo basePoolInfo)
    {    
        var name = RpcCaller.GetName(basePoolInfo.ProviderAddress);
        var objectToInstantiate = $"MetaDataAPI.Providers.{name}, MetaDataAPI";
        var objectType = Type.GetType(objectToInstantiate);
        return Activator.CreateInstance(objectType!, args: basePoolInfo ) as IProvider;
    }
    public Erc20Token GetErc20Token(string address) => new(address, RpcCaller);
}