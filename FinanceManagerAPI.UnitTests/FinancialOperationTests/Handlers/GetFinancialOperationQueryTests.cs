using FinanceManagerAPI.Application.FinancialOperationBehavior.Get;
using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceManagerAPI.UnitTests.FinancialOperationTests.Handlers;

[Collection("FinancialOperationHandlersTests")]
public class GetFinancialOperationQueryTests
{
    private readonly Mock<ILogger<GetFinancialOperationQueryHandler>> _mockLoggerForQuery;
    private readonly FinanceAPIDbContext _dbContext;

    public GetFinancialOperationQueryTests()
    {
        _dbContext = TestDbContext.CreateAndSeedTestDb();
        
        _mockLoggerForQuery = new Mock<ILogger<GetFinancialOperationQueryHandler>>(); 
    }

    [Fact]
    public async Task HandleGetFinancialOperation_ValidRequest_ReturnsFinancialOperationDtoAndAssertExistingInDB()
    {
        // Arrange
        _dbContext.FinancialOperations.Add(new FinancialOperation
        {
            Id = 3,
            Amount = 234.95M,
            DateTime = DateTime.UtcNow.Date,
            OperationTypeId = 3
        });
        await _dbContext.SaveChangesAsync();
        
        var existingFinancialOperationId = 3;
        
        var query = new GetFinancialOperationQuery(existingFinancialOperationId);

        var handler = new GetFinancialOperationQueryHandler(_dbContext, _mockLoggerForQuery.Object);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingFinancialOperationId);

        // Additional Assert for existing in the DB.
        var financialOperationFromDb = await _dbContext.FinancialOperations.FindAsync(existingFinancialOperationId);
        financialOperationFromDb.Should().NotBeNull();
        financialOperationFromDb!.Amount.Should().Be(234.95M);
        financialOperationFromDb.DateTime.Date.Should().Be(DateTime.UtcNow.Date);
        financialOperationFromDb.OperationTypeId.Should().Be(3);
    }
    
    [Fact]
    public async Task HandleGetFinancialOperation_InvalidRequest_OperationNotFound_ThrowsException()
    {
        // Arrange
        var invalidFinancialOperationId = 9999;
        
        var query = new GetFinancialOperationQuery(invalidFinancialOperationId);
        
        var handler = new GetFinancialOperationQueryHandler(_dbContext, _mockLoggerForQuery.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(query, default));
    }
}