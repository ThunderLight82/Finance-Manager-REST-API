using System.Net;
using FinanceManagerAPI.DTO.ModelsDTOs;
using FinanceManagerAPI.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;

namespace FinanceManagerAPI.UnitTests.FinancialOperationTests.Endpoints;

[Collection("FinancialOperationEndpointsTests")]
public class DeleteFinancialOperationEndpointTests
{
    private readonly TestServer _testServer;
    private readonly HttpClient _httpClient;

    public DeleteFinancialOperationEndpointTests()
    {
        _testServer = new TestServer(new WebHostBuilder()
            .ConfigureServices(services =>
                {
                    services.AddRouting();
                })
            .UseStartup<Startup>()
            .UseSerilog());

        _httpClient = _testServer.CreateClient();
    }
    
    [Fact]
    public async Task DeleteFinancialOperationEndpoint_EndpointValidation_InvalidRequests_ShouldReturnBadRequest()
    { 
        // Arrange
        var financialOperationDto = new FinancialOperationDto { Id = default };

        // Act
        var response = await _httpClient.DeleteAsync($"api/financial-operation/{financialOperationDto.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}