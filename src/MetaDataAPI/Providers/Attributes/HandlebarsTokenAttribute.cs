namespace MetaDataAPI.Providers.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HandlebarsTokenAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}