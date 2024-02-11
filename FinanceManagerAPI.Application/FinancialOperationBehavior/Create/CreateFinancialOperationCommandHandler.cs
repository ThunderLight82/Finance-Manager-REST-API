using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Create;

internal class CreateFinancialOperationCommandHandler : IRequestHandler<CreateFinancialOperationCommand>
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperationBehavior;
    private readonly ILogger<CreateFinancialOperationCommandHandler> _logger;

    public CreateFinancialOperationCommandHandler(IBaseBehavior<FinancialOperation> financialOperationBehavior, ILogger<CreateFinancialOperationCommandHandler> logger)
    {
        _financialOperationBehavior = financialOperationBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateFinancialOperationCommand request, CancellationToken cancellationToken)
    {
        var financialOperationDto = request.FinancialOperationDto;
        
        var newFinancialOperation = new FinancialOperation
        {
            DateTime = financialOperationDto.DateTime,
            Amount = financialOperationDto.Amount,
            OperationTypeId = financialOperationDto.OperationTypeDtoId
        };

        await _financialOperationBehavior.Create(newFinancialOperation);

        await _financialOperationBehavior.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Financial Operation with Id [{newFinancialOperation.Id}] was successfully created.");
        return Unit.Value;
    }
}