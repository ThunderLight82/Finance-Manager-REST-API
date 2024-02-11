using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Update;

internal class UpdateOperationTypeCommandHandler : IRequestHandler<UpdateOperationTypeCommand>
{
    private readonly IBaseBehavior<OperationType> _operationTypeBehavior;
    private readonly ILogger<UpdateOperationTypeCommandHandler> _logger;

    public UpdateOperationTypeCommandHandler(IBaseBehavior<OperationType> operationTypeBehavior, ILogger<UpdateOperationTypeCommandHandler> logger)
    {
        _operationTypeBehavior = operationTypeBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateOperationTypeCommand request, CancellationToken cancellationToken)
    {
        var operationTypeDto = request.OperationTypeDto;
        
        var existingOperationType = await _operationTypeBehavior.GetById(operationTypeDto.Id);
        
        if (existingOperationType == null)
        {
            _logger.LogError($"Error in OperationTypeBehavior - Update." +
                             $"Operation type with ID {operationTypeDto.Id} not found.");
            throw new Exception($"Operation Type with ID {operationTypeDto.Id} not found.");
        }

        existingOperationType.Name = operationTypeDto.Name;
        existingOperationType.IsIncomeOperation = operationTypeDto.IsIncomeOperation;
        
        await _operationTypeBehavior.Update(existingOperationType);

        await _operationTypeBehavior.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Operation Type with Id [{operationTypeDto.Id}] was successfully updated.");
        return Unit.Value;
    }
}