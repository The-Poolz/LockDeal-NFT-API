using FluentValidation;
using MetaDataAPI.Models;

namespace MetaDataAPI.Validation;

public class LambdaRequestValidator : AbstractValidator<LambdaRequest>
{
    public static readonly string[] AllowedMethods = ["GET", "OPTIONS"];

    public LambdaRequestValidator()
    {
        RuleFor(r => r.RawPath)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("RawPath is required (expected format: '/{chainId}/{poolId}').")
            .Must(HaveExactlyTwoSegments)
            .WithMessage(x => $"RawPath must be '/{{chainId}}/{{poolId}}'. The first path parameter is 'chainId', the second is 'poolId'. Received: '{x.RawPath}'.")
            .Must(BeValidChainId)
            .WithMessage(x => $"The first path parameter (chainId) must be a valid Int64. Received: '{GetSegment(x.RawPath, 0)}'.")
            .Must(BeValidPoolId)
            .WithMessage(x => $"The second path parameter (poolId) must be a valid Int64. Received: '{GetSegment(x.RawPath, 1)}'.");

        RuleFor(x => x.HttpMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("HTTP method is required.")
            .Must(m => AllowedMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
            .WithMessage(x => $"Allowed HTTP methods: ({string.Join(", ", AllowedMethods)}). Received HTTP method: {x.HttpMethod}");
    }

    private static string[] Split(string? rawPath) => (rawPath ?? string.Empty).Split('/', StringSplitOptions.RemoveEmptyEntries);
    private static bool HaveExactlyTwoSegments(string? rawPath) => Split(rawPath).Length == 2;
    private static bool BeValidChainId(string? rawPath) => long.TryParse(GetSegment(rawPath, 0), out _);
    private static bool BeValidPoolId(string? rawPath) => long.TryParse(GetSegment(rawPath, 1), out _);
    private static string GetSegment(string? rawPath, int index)
    {
        var parts = Split(rawPath);
        return parts.Length > index ? parts[index] : string.Empty;
    }
}