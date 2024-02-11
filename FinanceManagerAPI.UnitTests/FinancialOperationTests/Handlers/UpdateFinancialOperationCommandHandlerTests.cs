using FinanceManagerAPI.Application;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Update;
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

public class UpdateFinancialOperationCommandHandlerTests
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperation;
    private readonly Mock<ILogger<UpdateFinancialOperationCommandHandler>> _mockLoggerForHandler;
    private readonly FinanceAPIDbContext _dbContext;

    public UpdateFinancialOperationCommandHandlerTests()
    {
        _dbContext = TestDbContext.CreateAndSeedTestDb();
        
        _financialOperation = new TestFinancialOperationBehavior(_dbContext);
        _mockLoggerForHandler = new Mock<ILogger<UpdateFinancialOperationCommandHandler>>(); 
        _dbContext.FinancialOperations.RemoveRange(_dbContext.FinancialOperations);
    }
    
    private class TestFinancialOperationBehavior : BaseBehavior<FinancialOperation>
    {
        public TestFinancialOperationBehavior(FinanceAPIDbContext dbContext) : base(dbContext) { }
    }

    [Fact]
    public async Task HandleUpdateFinancialOperation_ValidRequest_ReturnUnitAndAssertUpdateInDB()
    {
        // Arrange
        var existingFinancialOperation = new FinancialOperation
        {
            Id = 4,
            Amount = 0M,
            DateTime = new DateTime(2007, 12, 21),
            OperationTypeId = 1
        };
        _dbContext.FinancialOperations.Add(existingFinancialOperation);
        await _dbContext.SaveChangesAsync();
        
        var updatedFinancialOperationDto = new FinancialOperationDto
        {
            Id = existingFinancialOperation.Id,
            DateTime = new DateTime(2024,1,30),
            Amount = 50M,
            OperationTypeDtoId = 2
        };
        
        var command = new UpdateFinancialOperationCommand(updatedFinancialOperationDto);

        var handler = new UpdateFinancialOperationCommandHandler(_financialOperation, _mockLoggerForHandler.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(Unit.Value);
        
        // Additional Assert for updating data from the DB.
        var updatedOperation = await _dbContext.FinancialOperations.FindAsync(updatedFinancialOperationDto.Id);
        updatedOperation.Should().NotBeNull();
        updatedOperation!.Amount.Should().Be(50M);
        updatedOperation.DateTime.Should().BeCloseTo(new DateTime(2024, 1, 30), precision: TimeSpan.FromSeconds(5));
        updatedOperation.OperationTypeId.Should().Be(2);
    }
    
    [Fact]
    public async Task HandleUpdateFinancialOperation_InvalidRequest_OperationNotFound_ThrowsException()
    {
        // Arrange
        var nonExistingFinancialOperationId = 9999;
        
        var command = new UpdateFinancialOperationCommand(new FinancialOperationDto { Id = nonExistingFinancialOperationId });
        
        var handler = new UpdateFinancialOperationCommandHandler(_financialOperation, _mockLoggerForHandler.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, default));
    }
}