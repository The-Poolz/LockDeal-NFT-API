using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MetaDataAPI.RPC.ABI.Models;

public class APIResponse
{
    public JArray ABI { get; set; } = new JArray();

    public string GetABI() => ABI.ToString(Formatting.None);
}