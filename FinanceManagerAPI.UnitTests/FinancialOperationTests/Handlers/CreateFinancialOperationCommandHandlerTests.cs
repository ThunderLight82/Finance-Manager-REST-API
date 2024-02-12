using FinanceManagerAPI.Application;
using FinanceManagerAPI.Application.FinancialOperationBehavior.Create;
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

[Collection("FinancialOperationHandlersTests")]
public class CreateFinancialOperationCommandHandlerTests
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperation;
    private readonly Mock<ILogger<CreateFinancialOperationCommandHandler>> _mockLoggerForHandler;
    private readonly FinanceAPIDbContext _dbContext;

    public CreateFinancialOperationCommandHandlerTests()
    {
        _dbContext = TestDbContext.CreateAndSeedTestDb();
        
        _financialOperation = new TestFinancialOperationBehavior(_dbContext);
        _mockLoggerForHandler = new Mock<ILogger<CreateFinancialOperationCommandHandler>>(); 
    }
    
    private class TestFinancialOperationBehavior : BaseBehavior<FinancialOperation>
    {
        public TestFinancialOperationBehavior(FinanceAPIDbContext dbContext) : base(dbContext) { }
    }

    [Fact]
    public async Task HandleCreateFinancialOperation_ValidRequest_ReturnUnitAndAssertCreationInDB()
    {
        // Arrange
        var financialOperationDto = new FinancialOperationDto
        {
            DateTime = DateTime.UtcNow,
            Amount = 100.75M,
            OperationTypeDtoId = 1
        };
        
        var command = new CreateFinancialOperationCommand(financialOperationDto);

        var handler = new CreateFinancialOperationCommandHandler(_financialOperation, _mockLoggerForHandler.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(Unit.Value);

        // Additional Assert for creation in the DB.
        var createdOperation = _dbContext.FinancialOperations.First();
        createdOperation.Amount.Should().Be(100.75M); 
        createdOperation.OperationTypeId.Should().Be(1);
    }
}