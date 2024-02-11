using System.Net;
using System.Text;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Create;
using FinanceManagerAPI.DTO.ModelsDTOs;
using FinanceManagerAPI.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Xunit;

namespace FinanceManagerAPI.UnitTests.FinancialOperationTests.Endpoints;

public class CreateFinancialOperationEndpointTests
{
    private readonly TestServer _testServer;
    private readonly HttpClient _httpClient;

    public CreateFinancialOperationEndpointTests()
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
    
    [Theory]
    [InlineData(-10, "2024-02-05T12:34:56Z", 1, "Error: Value [Amount] should be greater that 0.")]     // Negative in Amount
    [InlineData(0, "2024-02-05T12:34:56Z", 12, "Error: Value [Amount] shouldn't be empty or 0.")]       // 0 in Amount
    [InlineData(default, "2024-02-05T12:34:56Z", 23, "Error: Value [Amount] shouldn't be empty or 0.")] // Empty in Amount
    [InlineData(100.05, "2024-02-05T12:34:56Z", default, "Error: Value [Type] shouldn't be empty.")]    // Empty in  OperationTypeDtoId
    public async Task CreateFinancialOperationEndpoint_DifferentEndpointsValidations_InvalidRequests_ShouldReturnBadRequestWithMessage
        (decimal amount, DateTime dateTime, int operationTypeDtoId, string expectedErrorMessage)
    { 
        // Arrange
        var invalidFinancialOperationDto = new FinancialOperationDto
        {
            DateTime = dateTime,
            Amount = amount,
            OperationTypeDtoId = operationTypeDtoId
        };

        var invalidCommand = new CreateFinancialOperationCommand(invalidFinancialOperationDto);
        
        var content = new StringContent(JsonConvert.SerializeObject(invalidCommand), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/financial-operation", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent.Should().Contain(expectedErrorMessage);
    }
}