using FinanceManagerAPI.Application;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Delete;
using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;
using FinanceManagerAPI.DTO.ModelsDTOs;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FinanceManagerAPI.UnitTests.FinancialOperationTests.Handlers;

public class DeleteFinancialOperationCommandHandlerTests
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperation;
    private readonly Mock<ILogger<DeleteFinancialOperationCommandHandler>> _mockLoggerForHandler;
    private readonly FinanceAPIDbContext _dbContext;

    public DeleteFinancialOperationCommandHandlerTests()
    {
        _dbContext = TestDbContext.CreateAndSeedTestDb();
        
        _financialOperation = new TestFinancialOperationBehavior(_dbContext);
        _mockLoggerForHandler = new Mock<ILogger<DeleteFinancialOperationCommandHandler>>(); 
    }
    
    private class TestFinancialOperationBehavior : BaseBehavior<FinancialOperation>
    {
        public TestFinancialOperationBehavior(FinanceAPIDbContext dbContext) : base(dbContext) { }
    }

    [Fact]
    public async Task HandleDeleteFinancialOperation_ValidRequest_ReturnUnitAndAssertDeletionInDB()
    {
        // Arrange
        var financialOperationDto = new FinancialOperationDto { Id = 2 };
        
        await _dbContext.FinancialOperations.AddAsync(new FinancialOperation { Id = 2 });
        await _dbContext.SaveChangesAsync();
        
        var command = new DeleteFinancialOperationCommand(financialOperationDto);

        var handler = new DeleteFinancialOperationCommandHandler(_financialOperation, _mockLoggerForHandler.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(Unit.Value);

        // Additional Assert for deletion from the DB.
        _dbContext.FinancialOperations.Should().NotContain(f => f.Id == 2);
    }
    
    [Fact]
    public async Task HandleDeleteFinancialOperation_InvalidRequest_OperationNotFound_ThrowsException()
    {
        // Arrange
        var nonExistingFinancialOperationId = 9999;
        
        var command = new DeleteFinancialOperationCommand(new FinancialOperationDto { Id = nonExistingFinancialOperationId });
        
        var handler = new DeleteFinancialOperationCommandHandler(_financialOperation, _mockLoggerForHandler.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, default));
    }
}