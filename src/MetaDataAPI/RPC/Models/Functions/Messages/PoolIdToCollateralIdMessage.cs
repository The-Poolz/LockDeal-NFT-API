using System.Numerics;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.Functions.Messages;

[Function("poolIdToCollateralId", "uint256")]
public class PoolIdToCollateralIdMessage : FunctionMessage
{
    [Parameter("uint256", "", 1)]
    public virtual BigInteger PoolId { get; set; }
}