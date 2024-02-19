using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Update;

internal class UpdateFinancialOperationCommandHandler : IRequestHandler<UpdateFinancialOperationCommand>
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperationBehavior;
    private readonly ILogger<UpdateFinancialOperationCommandHandler> _logger;

    public UpdateFinancialOperationCommandHandler(IBaseBehavior<FinancialOperation> financialOperationBehavior, ILogger<UpdateFinancialOperationCommandHandler> logger)
    {
        _financialOperationBehavior = financialOperationBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateFinancialOperationCommand request, CancellationToken cancellationToken)
    {
        var financialOperationDto = request.FinancialOperationDto;
        
        var existingOperation = await _financialOperationBehavior.GetById(financialOperationDto.Id);
        
        if (existingOperation == null)
        {
            _logger.LogError($"Error in FinancialOperationBehavior - Update." +
                             $"Financial operation with ID {financialOperationDto.Id} not found.");
            throw new ArgumentNullException($"Financial operation with ID {financialOperationDto.Id} not found.");
        }

        existingOperation.Amount = financialOperationDto.Amount;
        existingOperation.DateTime = financialOperationDto.DateTime;
        existingOperation.OperationTypeId = financialOperationDto.OperationTypeDtoId;

        await _financialOperationBehavior.Update(existingOperation);

        await _financialOperationBehavior.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Financial Operation with Id [{financialOperationDto.Id}] was successfully updated.");
        return Unit.Value;
    }
}