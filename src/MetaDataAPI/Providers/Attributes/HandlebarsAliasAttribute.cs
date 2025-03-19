namespace MetaDataAPI.Providers.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HandlebarsAliasAttribute(string alias) : Attribute
{
    public string Alias { get; } = alias;
}