using System.Numerics;
using Nethereum.Contracts;
using MetaDataAPI.RPC.Models.Functions.Outputs;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.Functions.Messages;

[Function("getFullData", typeof(GetFullDataOutput))]
public class GetFullDataMessage : FunctionMessage
{
    [Parameter("uint256", "poolId", 1)]
    public virtual BigInteger PoolId { get; set; }
}