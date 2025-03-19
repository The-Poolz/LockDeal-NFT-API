namespace MetaDataAPI.Providers.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HandlebarsLabelAttribute(string labelName, int order) : Attribute
{
    public string LabelName { get; } = labelName;
    public int Order { get; } = order;
}