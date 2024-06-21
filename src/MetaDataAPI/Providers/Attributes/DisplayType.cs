using System.Runtime.Serialization;

namespace MetaDataAPI.Providers.Attributes;

public enum DisplayType
{
    [EnumMember(Value = "string")]
    String,
    [EnumMember(Value = "number")]
    Number,
    [EnumMember(Value = "date")]
    Date
}