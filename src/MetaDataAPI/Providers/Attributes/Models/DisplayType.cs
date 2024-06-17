using System.Runtime.Serialization;

namespace MetaDataAPI.Providers.Attributes.Models;

public enum DisplayType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "string")]
    String,
    [EnumMember(Value = "number")]
    Number,
    [EnumMember(Value = "date")]
    Date
}