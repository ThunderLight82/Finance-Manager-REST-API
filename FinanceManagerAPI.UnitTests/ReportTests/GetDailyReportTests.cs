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
        var financialOperation = new FinancialOperation
        {
            Id = 5,
            Amount = 50.50M,
            DateTime = new DateTime(2024, 1, 10),
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

        var query = new GetDailyReportQuery(new DateTime(2024, 1, 10));
        
        var handler = new GetDailyReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        var result = await handler.Handle(query, default);

        result.Should().NotBeNull();
        result.TotalIncome.Should().Be(50.50M);
        result.TotalExpenses.Should().Be(0M);
        result.Operations.Should().ContainSingle().Which.Id.Should().Be(5);
    }
    
    [Fact]
    public async Task HandleGetDailyReport_MultipleOperations_ValidRequest_ReturnsDailyReportResponseAndAssertExistingInDB()
    {

        var financialOperations = new List<FinancialOperation>
        {
            new (id: 6, amount: 20M, dateTime: DateTime.UtcNow.Date, operationTypeId: 6),
            new (id: 7, amount: 20M, dateTime: DateTime.UtcNow.Date, operationTypeId: 7),
            new (id: 8, amount: 70.99M, dateTime: DateTime.UtcNow.Date, operationTypeId: 8),
            new (id: 9, amount: 100M, dateTime: DateTime.UtcNow.Date, operationTypeId: 9)
        };

        var operationTypes = new List<OperationType>
        {
            new (id: 6, name: "Income 1", isIncomeOperation: true),
            new (id: 7, name: "Income 2", isIncomeOperation: true),
            new (id: 8, name: "Expense 1", isIncomeOperation: false),
            new (id: 9, name: "Expense 2", isIncomeOperation: false)
        };

        _dbContext.FinancialOperations.AddRange(financialOperations);
        _dbContext.OperationsTypes.AddRange(operationTypes);
        await _dbContext.SaveChangesAsync();
        
        var query = new GetDailyReportQuery(DateTime.UtcNow.Date);
        
        var handler = new GetDailyReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        var result = await handler.Handle(query, default);

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