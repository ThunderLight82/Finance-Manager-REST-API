using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Delete;

internal class DeleteFinancialOperationCommandHandler : IRequestHandler<DeleteFinancialOperationCommand>
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperationBehavior;
    private readonly ILogger<DeleteFinancialOperationCommandHandler> _logger;

    public DeleteFinancialOperationCommandHandler(IBaseBehavior<FinancialOperation> financialOperationBehavior, ILogger<DeleteFinancialOperationCommandHandler> logger)
    {
        _financialOperationBehavior = financialOperationBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteFinancialOperationCommand request, CancellationToken cancellationToken)
    {
        var financialOperationDto = request.FinancialOperationDto;
        
        var getOperation = await _financialOperationBehavior.GetById(financialOperationDto.Id);
        
        if (getOperation == null)
        {
            _logger.LogError($"Error in FinancialOperationBehavior - Delete." +
                             $"Financial operation with ID {financialOperationDto.Id} not found.");
            throw new ArgumentNullException($"Financial operation with ID {financialOperationDto.Id} not found.");
        }
        
        await _financialOperationBehavior.Delete(getOperation);

        await _financialOperationBehavior.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation($"Financial Operation with Id [{financialOperationDto.Id}] was successfully deleted.");
        return Unit.Value;
    }
}