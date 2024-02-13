using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.Functions.Outputs;

public class BasePoolInfo
{
    [Parameter("address", "provider")]
    public virtual string Provider { get; set; } = string.Empty;

    [Parameter("string", "name", 2)]
    public virtual string Name { get; set; } = string.Empty;

    [Parameter("uint256", "poolId", 3)]
    public virtual BigInteger PoolId { get; set; }

    [Parameter("uint256", "vaultId", 4)]
    public virtual BigInteger VaultId { get; set; }

    [Parameter("address", "owner", 5)]
    public virtual string Owner { get; set; } = string.Empty;

    [Parameter("address", "token", 6)]
    public virtual string Token { get; set; } = string.Empty;

    [Parameter("uint256[]", "params", 7)]
    public virtual List<BigInteger> Params { get; set; } = new List<BigInteger>();
}