using Newtonsoft.Json;
using System.Numerics;

namespace MetaDataAPI
{
    public class BasePoolInfo
    {
        public Provider Provider { get; set; }
        public BigInteger PoolId { get; set; }
        public string Owner { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public List<BigInteger> Params { get; set; }
        [JsonIgnore]
        public List<string> Keys => Provider.ParamsName;
        public Dictionary<string, BigInteger> ParamsDict
        {
            get
            {
                if (Keys.Count != Params.Count)
                {
                    throw new InvalidOperationException("Mismatch between keys and params counts");
                }

                return Keys.Zip(Params, (k, v) => new { Key = k, Value = v })
                           .ToDictionary(x => x.Key, x => x.Value);
            }
        }
    }

}
