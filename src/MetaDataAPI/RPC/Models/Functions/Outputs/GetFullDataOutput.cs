using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.Functions.Outputs;

[FunctionOutput]
public class GetFullDataOutput : IFunctionOutputDTO
{
    [Parameter("tuple[]", "poolInfo")]
    public virtual List<BasePoolInfo> PoolInfo { get; set; } = new List<BasePoolInfo>();
}