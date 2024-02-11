using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Delete;

internal class DeleteOperationTypeCommandHandler : IRequestHandler<DeleteOperationTypeCommand>
{
    private readonly IBaseBehavior<OperationType> _operationTypeBehavior;
    private readonly ILogger<DeleteOperationTypeCommandHandler> _logger;

    public DeleteOperationTypeCommandHandler(IBaseBehavior<OperationType> operationTypeBehavior, ILogger<DeleteOperationTypeCommandHandler> logger)
    {
        _operationTypeBehavior = operationTypeBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOperationTypeCommand request, CancellationToken cancellationToken)
    {
        var operationTypeDto = request.OperationTypeDto;
        
        var getOperation = await _operationTypeBehavior.GetById(operationTypeDto.Id);
        
        if (getOperation == null)
        {
            _logger.LogError($"Error in OperationTypeBehavior - Delete." +
                             $"Operation type with ID {operationTypeDto.Id} not found.");
            throw new Exception($"Operation Type with ID {operationTypeDto.Id} not found.");
        }
        
        await _operationTypeBehavior.Delete(getOperation);

        await _operationTypeBehavior.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Operation Type with Id [{operationTypeDto.Id}] was successfully deleted.");
        return Unit.Value;
    }
}