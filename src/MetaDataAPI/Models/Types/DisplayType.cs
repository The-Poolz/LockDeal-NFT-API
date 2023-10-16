using System.Runtime.Serialization;

namespace MetaDataAPI.Models.Types;

public enum DisplayType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "string")]
    String,
    [EnumMember(Value = "number")]
    Number,
    [EnumMember(Value = "date")]
    Date,
    [EnumMember(Value = "ignore")]
    Ignore
}