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

    [Fact]
    public void ShouldHaveError_WhenHttpMethodIsMissing()
    {
        var albRequest = new ApplicationLoadBalancerRequest();
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.HttpMethod)
            .WithErrorMessage(ValidatorErrorsMessages.HttpMethodRequired());
    }

    [Fact]
    public void ShouldHaveError_WhenHttpMethodIsNotAllowed()
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = "POST"
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.HttpMethod)
            .WithErrorMessage(ValidatorErrorsMessages.HttpMethodNotAllowed(albRequest.HttpMethod, LambdaRoutes.AllowedMethods));
    }

    [Fact]
    public void ShouldValidateSuccessfully_WhenHttpMethodIsOptions()
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.OptionsMethod
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ShouldHaveError_WhenPathIsMissing()
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.GetMethod,
            Path = string.Empty
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.Path)
            .WithErrorMessage(ValidatorErrorsMessages.PathRequired());
    }

    [Fact]
    public void ShouldHaveError_WhenPathIsNotAllowed()
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.GetMethod,
            Path = "/not-allowed"
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Request.Path)
            .WithErrorMessage(ValidatorErrorsMessages.PathNotAllowed(albRequest.Path));
    }

    [Theory]
    [InlineData("/metadata/1/2")]
    [InlineData("/favicon.ico")]
    [InlineData("/health")]
    public void ShouldValidateSuccessfully_ForAllowedPaths(string path)
    {
        var albRequest = new ApplicationLoadBalancerRequest
        {
            HttpMethod = LambdaRoutes.GetMethod,
            Path = path
        };
        var request = new RouteApplicationLoadBalancerRequest(albRequest);

        var result = _validator.TestValidate(request);

        result.IsValid.Should().BeTrue();
    }
}