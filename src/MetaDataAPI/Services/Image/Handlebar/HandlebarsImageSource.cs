using System.Numerics;

namespace MetaDataAPI.Services.Image.Handlebar;

public class HandlebarsImageSource(
    BigInteger poolId,
    string providerName,
    HandlebarsToken firstToken,
    HandlebarsToken? secondToken = null,
    HandlebarsLabel? firstLabel = null,
    HandlebarsLabel? secondLabel = null,
    HandlebarsLabel? thirdLabel = null,
    HandlebarsLabel? fourthLabel = null
)
{
    public BigInteger PoolId { get; } = poolId;
    public string ProviderName { get; } = providerName;

    public string? FirstLabelName { get; } = firstLabel?.Name;
    public object? FirstLabelValue { get; } = firstLabel?.Value;

    public string? SecondLabelName { get; } = secondLabel?.Name;
    public object? SecondLabelValue { get; } = secondLabel?.Value;

    public string? ThirdLabelName { get; } = thirdLabel?.Name;
    public object? ThirdLabelValue { get; } = thirdLabel?.Value;

    public string? FourthLabelName { get; } = fourthLabel?.Name;
    public object? FourthLabelValue { get; } = fourthLabel?.Value;

    public string FirstTokenName { get; } = firstToken.Name;
    public object FirstTokenLabel { get; } = firstToken.Label;
    public object FirstTokenValue { get; } = firstToken.Value;

    public string? SecondTokenName { get; } = secondToken?.Name;
    public object? SecondTokenLabel { get; } = secondToken?.Label;
    public object? SecondTokenValue { get; } = secondToken?.Value;
}