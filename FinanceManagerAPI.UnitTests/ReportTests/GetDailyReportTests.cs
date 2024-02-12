using FinanceManagerAPI.Application.ReportBehavior.DailyReport;
using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceManagerAPI.UnitTests.ReportTests;

[Collection("ReportTests")]
public class GetDailyReportTests
{
    private readonly Mock<ILogger<GetDailyReportQueryHandler>> _mockLoggerForQuery;
    private readonly FinanceAPIDbContext _dbContext;

    public GetDailyReportTests()
    {
        _dbContext = TestDbContext.CreateAndSeedTestDb();
        
        _mockLoggerForQuery = new Mock<ILogger<GetDailyReportQueryHandler>>(); 
    }

    [Fact]
    public async Task HandleGetDailyReport_OneOperation_ValidRequest_ReturnsDailyReportResponseAndAssertExistingInDB()
    {
        // Arrange
        var financialOperation = new FinancialOperation
        {
            Id = 5,
            Amount = 50.50M,
            DateTime = new DateTime(2022, 1, 10),
            OperationTypeId = 5
        };
        
        var operationType = new OperationType
        {
            Id = 5,
            Name = "Test Income",
            IsIncomeOperation = true
        };

        _dbContext.FinancialOperations.Add(financialOperation);
        _dbContext.OperationsTypes.Add(operationType);
        await _dbContext.SaveChangesAsync();

        var query = new GetDailyReportQuery(new DateTime(2022, 1, 10));
        
        var handler = new GetDailyReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.TotalIncome.Should().Be(50.50M);
        result.TotalExpenses.Should().Be(0M);
        result.Operations.Should().ContainSingle().Which.Id.Should().Be(5);
    }
    
    [Fact]
    public async Task HandleGetDailyReport_MultipleOperations_ValidRequest_ReturnsDailyReportResponseAndAssertExistingInDB()
    {
        // Arrange
        var financialOperations = new List<FinancialOperation>
        {
            new() {Id = 6, Amount = 20M, DateTime = DateTime.UtcNow.Date, OperationTypeId = 6},
            new() {Id = 7, Amount = 20M, DateTime = DateTime.UtcNow.Date, OperationTypeId = 7},
            new() {Id = 8, Amount = 70.99M, DateTime = DateTime.UtcNow.Date, OperationTypeId = 8},
            new() {Id = 9, Amount = 100M, DateTime = DateTime.UtcNow.Date, OperationTypeId = 9}
        };

        var operationTypes = new List<OperationType>
        {
            new() {Id = 6, Name = "Income 1", IsIncomeOperation = true},
            new() {Id = 7, Name = "Income 2", IsIncomeOperation = true},
            new() {Id = 8, Name = "Expense 1", IsIncomeOperation = false},
            new() {Id = 9, Name = "Expense 2", IsIncomeOperation = false}
        };

        _dbContext.FinancialOperations.AddRange(financialOperations);
        _dbContext.OperationsTypes.AddRange(operationTypes);
        await _dbContext.SaveChangesAsync();
        
        var query = new GetDailyReportQuery(DateTime.UtcNow.Date);
        
        var handler = new GetDailyReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.TotalIncome.Should().Be(40M);
        result.TotalExpenses.Should().Be(170.99M);
        result.Operations.Should().HaveCount(4);
    }
    
    [Fact]
    public async Task HandleGetDailyReport_InvalidRequest_OperationsNotFound_ThrowsException()
    {
        // Arrange
        var query = new GetDailyReportQuery(new DateTime(2007, 1, 1));
        
        var handler = new GetDailyReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(query, default));
    }
}