using Xunit;
using FluentAssertions;
using MetaDataAPI.Routing;
using MetaDataAPI.Validation;
using FluentValidation.TestHelper;
using MetaDataAPI.Routing.Requests;
using Amazon.Lambda.ApplicationLoadBalancerEvents;

namespace MetaDataAPI.Tests.Validation;

public class RouteApplicationLoadBalancerRequestValidatorTests
{
    private readonly RouteApplicationLoadBalancerRequestValidator _validator = new();
    private readonly ApplicationLoadBalancerRequest _albRequest = new();

    [Fact]
    public void ShouldHaveError_WhenHttpMethodIsMissing()
    {
        var request = new RouteApplicationLoadBalancerRequest(_albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.HttpMethod)
            .WithErrorMessage(ValidatorErrorsMessages.HttpMethodRequired());
    }

    [Fact]
    public void ShouldHaveError_WhenHttpMethodIsNotAllowed()
    {
        _albRequest.HttpMethod = "POST";
        var request = new RouteApplicationLoadBalancerRequest(_albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.HttpMethod)
            .WithErrorMessage(ValidatorErrorsMessages.HttpMethodNotAllowed(_albRequest.HttpMethod, LambdaRoutes.AllowedMethods));
    }

    [Fact]
    public void ShouldValidateSuccessfully_WhenHttpMethodIsOptions()
    {
        _albRequest.HttpMethod = LambdaRoutes.OptionsMethod;
        var request = new RouteApplicationLoadBalancerRequest(_albRequest);

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ShouldHaveError_WhenPathIsMissing()
    {
        _albRequest.HttpMethod = LambdaRoutes.GetMethod;
        _albRequest.Path = string.Empty;
        var request = new RouteApplicationLoadBalancerRequest(_albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.Path)
            .WithErrorMessage(ValidatorErrorsMessages.PathRequired());
    }

    [Fact]
    public void ShouldHaveError_WhenPathIsNotAllowed()
    {
        _albRequest.HttpMethod = LambdaRoutes.GetMethod;
        _albRequest.Path = "/not-allowed";
        var request = new RouteApplicationLoadBalancerRequest(_albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.Path)
            .WithErrorMessage(ValidatorErrorsMessages.PathNotAllowed(_albRequest.Path));
    }

    [Theory]
    [InlineData("/metadata/1/2")]
    [InlineData("/favicon.ico")]
    public void ShouldValidateSuccessfully_ForAllowedPaths(string path)
    {
        _albRequest.HttpMethod = LambdaRoutes.GetMethod;
        _albRequest.Path = path;
        var request = new RouteApplicationLoadBalancerRequest(_albRequest);

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }
}