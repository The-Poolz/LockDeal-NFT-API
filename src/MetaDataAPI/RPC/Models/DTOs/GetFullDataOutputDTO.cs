using Nethereum.ABI.FunctionEncoding.Attributes;

namespace MetaDataAPI.RPC.Models.DTOs;

[FunctionOutput]
public class GetFullDataOutputDTO : IFunctionOutputDTO
{
    [Parameter("tuple[]", "poolInfo")]
    public virtual List<BasePoolInfoDTO> PoolInfo { get; set; } = new List<BasePoolInfoDTO>();
}