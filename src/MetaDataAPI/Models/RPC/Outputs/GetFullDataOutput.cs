using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.Models.RPC.Outputs;

[FunctionOutput]
public class GetFullDataOutput : IFunctionOutputDTO
{
    [Parameter("tuple[]", "poolInfo")]
    public virtual List<BasePoolInfoDTO> PoolInfo { get; set; } = new List<BasePoolInfoDTO>();
}