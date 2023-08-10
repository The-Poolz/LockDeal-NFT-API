using MetaDataAPI.Providers;
using Newtonsoft.Json;

namespace MetaDataAPI;

public class Provider
{
    public Provider(string rawAddress)
    {
        Address = "0x" + rawAddress.Substring(24);
        Name = ProviderNames[Address].ToString();
    }
    public string Address { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<string> ParamsName => GetProvider().ParamsName;
    internal IProvider GetProvider()
    {
        return Name switch
        {
            nameof(ProviderName.Deal) => new Deal(),
            nameof(ProviderName.Lock) => new Lock(),
            nameof(ProviderName.Timed) => new Timed(),
            nameof(ProviderName.Refund) => new Refund(),
            nameof(ProviderName.Bundle) => new Bundle(),
            nameof(ProviderName.Collateral) => new Collateral(),
            _ => throw new Exception("Unknown provider")
        };
    }       
    internal enum ProviderName { Deal, Lock, Timed, Refund, Bundle, Collateral}
    internal Dictionary<string, ProviderName> ProviderNames => new()
    {
        { "0x2028C98AC1702E2bb934A3E88734ccaE42d44338".ToLower() , ProviderName.Deal },
        { "0xD5dF3f41Cc1Df2cc42F3b683dD71eCc38913e0d6".ToLower() , ProviderName.Lock },
        { "0x5C0cB6dd68102f51DC112c3ceC1c7090D27853bc".ToLower() , ProviderName.Timed },
        { "0x5eBa5A16A42241D4E1d427C9EC1E4C0AeC67e2A2".ToLower() , ProviderName.Refund },
        { "0xF1Ce27BD46F1f94Ce8Dc4DE4C52d3D845EfC29F0".ToLower() , ProviderName.Bundle },
        { "0xDB65cE03690e7044Ac12F5e2Ab640E7A355E9407".ToLower() , ProviderName.Collateral }
    };
}
