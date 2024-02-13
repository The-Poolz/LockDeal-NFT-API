using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.Functions.Outputs;

[FunctionOutput]
public class PoolIdToCollateralIdOutput : IFunctionOutputDTO
{
    [Parameter("uint256", "", 1)]
    public virtual BigInteger CollateralId { get; set; }
}