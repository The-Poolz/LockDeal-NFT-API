namespace MetaDataAPI.Services.Image.Handlebar;

[AttributeUsage(AttributeTargets.Property)]
public class HandlebarsMemberAttribute : Attribute
{
    public HandlebarsMemberType MemberType { get; }
    public string? Alias { get; }
    public string? LabelName { get; }
    public int Order { get; }

    public HandlebarsMemberAttribute(string alias)
    {
        MemberType = HandlebarsMemberType.Alias;
        Alias = alias;
    }

    public HandlebarsMemberAttribute(string labelName, int order)
    {
        MemberType = HandlebarsMemberType.Label;
        LabelName = labelName;
        Order = order;
    }

    public HandlebarsMemberAttribute(int order)
    {
        MemberType = HandlebarsMemberType.Token;
        Order = order;
    }
}