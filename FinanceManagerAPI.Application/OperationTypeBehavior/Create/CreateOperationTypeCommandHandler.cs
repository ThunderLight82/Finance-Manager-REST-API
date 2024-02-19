using FinanceManagerAPI.Application.FinancialOperationBehavior.Create;
using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Create;

internal class CreateOperationTypeCommandHandler : IRequestHandler<CreateOperationTypeCommand>
{
    private readonly IBaseBehavior<OperationType> _operationTypeBehavior;
    private readonly ILogger<CreateFinancialOperationCommandHandler> _logger;

    public CreateOperationTypeCommandHandler(IBaseBehavior<OperationType> operationTypeBehavior, ILogger<CreateFinancialOperationCommandHandler> logger)
    {
        _operationTypeBehavior = operationTypeBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateOperationTypeCommand request, CancellationToken cancellationToken)
    {
        var operationTypeDto = request.OperationTypeDto;
        
        var newOperationType = new OperationType
        {
            Name = operationTypeDto.Name,
            IsIncomeOperation = operationTypeDto.IsIncomeOperation
        };

        await _operationTypeBehavior.Create(newOperationType);

        await _operationTypeBehavior.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Operation Type with Id [{newOperationType.Id}] was successfully created.");
        return Unit.Value;
    }
}