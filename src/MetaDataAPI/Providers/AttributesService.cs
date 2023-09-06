using System.Numerics;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public static class AttributesService
{
    public static IEnumerable<Erc721Attribute> GetProviderAttributes(BigInteger poolId) =>  
         new BasePoolInfo(RpcCaller.GetMetadata(poolId)).Provider.Attributes;

    public static Erc721Attribute GetMainCoinAttribute(BigInteger collateralId) =>
        new("MainCoin", GetAddress(collateralId + 1));

    public static Erc721Attribute GetTokenAttribute(BigInteger collateralId) =>
        new("Token", GetAddress(collateralId + 2));

    private static string GetAddress(BigInteger poolId) =>
        new BasePoolInfo(RpcCaller.GetMetadata(poolId)).Token.Address;
}