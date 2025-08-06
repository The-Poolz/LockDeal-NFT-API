using System.Runtime.Serialization;

namespace MetaDataAPI.Providers.Attributes;

public enum DisplayType
{
    [EnumMember(Value = "number")]
    Number,
    [EnumMember(Value = "date")]
    Date
}