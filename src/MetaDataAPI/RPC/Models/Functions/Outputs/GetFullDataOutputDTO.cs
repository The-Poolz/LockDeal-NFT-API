using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.Functions.Outputs;

[FunctionOutput]
public class GetFullDataOutputDTO : IFunctionOutputDTO
{
    [Parameter("tuple[]", "poolInfo")]
    public virtual List<BasePoolInfoDTO> PoolInfo { get; set; } = new List<BasePoolInfoDTO>();
}