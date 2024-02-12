using FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;
using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceManagerAPI.UnitTests.ReportTests;

[Collection("ReportTests")]
public class GetDatePeriodReportTests
{
    private readonly Mock<ILogger<GetDatePeriodReportQueryHandler>> _mockLoggerForQuery;
    private readonly FinanceAPIDbContext _dbContext;

    public GetDatePeriodReportTests()
    {
        _dbContext = TestDbContext.CreateAndSeedTestDb();
        
        _mockLoggerForQuery = new Mock<ILogger<GetDatePeriodReportQueryHandler>>();
    }
    
     [Fact]
    public async Task HandleDatePeriodReport_OneOperationWithinDates_ValidRequest_ReturnsPeriodReportResponseAndAssertExistingInDB()
    {
        // Arrange
        var financialOperation = new FinancialOperation
        {
            Id = 10,
            Amount = 777M,
            DateTime = new DateTime(2020, 1, 9),
            OperationTypeId = 10
        };
        
        var operationType = new OperationType
        {
            Id = 10,
            Name = "Test Income",
            IsIncomeOperation = true
        };

        _dbContext.FinancialOperations.Add(financialOperation);
        _dbContext.OperationsTypes.Add(operationType);
        await _dbContext.SaveChangesAsync();

        var query = new GetDatePeriodReportQuery(new DateTime(2020, 1, 1), new DateTime(2020, 1, 15));
        
        var handler = new GetDatePeriodReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.TotalIncome.Should().Be(777M);
        result.TotalExpenses.Should().Be(0M);
        result.Operations.Should().ContainSingle().Which.Id.Should().Be(10);
    }
    
    [Fact]
    public async Task HandleDatePeriodReport_MultipleOperationWithinDates_ValidRequest_ReturnsPeriodReportResponseAndAssertExistingInDB()
    {
        // Arrange
        var financialOperations = new List<FinancialOperation>
        {
            new() {Id = 11, Amount = 50M, DateTime = new DateTime(2023, 2, 1), OperationTypeId = 11},
            new() {Id = 12, Amount = 50M, DateTime = new DateTime(2023, 2, 9), OperationTypeId = 12},
            new() {Id = 13, Amount = 300.99M, DateTime = new DateTime(2023, 2, 17), OperationTypeId = 13},
            new() {Id = 14, Amount = 1000M, DateTime = new DateTime(2023, 2, 2), OperationTypeId = 14}
        };

        var operationTypes = new List<OperationType>
        {
            new() {Id = 11, Name = "Income 1", IsIncomeOperation = true},
            new() {Id = 12, Name = "Income 2", IsIncomeOperation = true},
            new() {Id = 13, Name = "Expense 1", IsIncomeOperation = false},
            new() {Id = 14, Name = "Expense 2", IsIncomeOperation = false}
        };

        _dbContext.FinancialOperations.AddRange(financialOperations);
        _dbContext.OperationsTypes.AddRange(operationTypes);
        await _dbContext.SaveChangesAsync();
        
        var query = new GetDatePeriodReportQuery(new DateTime(2023, 2, 1), new DateTime(2023, 2, 28));
        
        var handler = new GetDatePeriodReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.TotalIncome.Should().Be(100M);
        result.TotalExpenses.Should().Be(1300.99M);
        result.Operations.Should().HaveCount(4);
    }
    
    [Fact]
    public async Task HandleDatePeriodReport_InvalidRequest_OperationsNotFound_ThrowsException()
    {
        // Arrange
        var query = new GetDatePeriodReportQuery(new DateTime(2007, 1, 1), new DateTime(2007, 2, 1));
        
        var handler = new GetDatePeriodReportQueryHandler(_dbContext, _mockLoggerForQuery.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(query, default));
    }
}