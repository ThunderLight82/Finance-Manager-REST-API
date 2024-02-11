using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Get;

internal class GetFinancialOperationQueryHandler : IRequestHandler<GetFinancialOperationQuery, FinancialOperationDto>
{
    private readonly FinanceAPIDbContext _dbContext;
    private readonly ILogger<GetFinancialOperationQueryHandler> _logger;

    public GetFinancialOperationQueryHandler(FinanceAPIDbContext dbContext, ILogger<GetFinancialOperationQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<FinancialOperationDto> Handle(GetFinancialOperationQuery request, CancellationToken cancellationToken)
    {
        var financialOperationId = request.FinancialOperationId;

        var getFinancialOperation = await _dbContext.FinancialOperations
            .Where(fo => fo.Id == financialOperationId)
            .FirstOrDefaultAsync(cancellationToken);
            
        if (getFinancialOperation == null)
        {
            _logger.LogError($"Error in FinancialOperationBehavior - Get." +
                             $"Financial operation with ID {request.FinancialOperationId} not found.");
            throw new ArgumentNullException($"Financial operation with ID {request.FinancialOperationId} not found.");
        }

        return new FinancialOperationDto
        {
            Id = getFinancialOperation.Id,
            DateTime = getFinancialOperation.DateTime,
            Amount = getFinancialOperation.Amount,
            OperationTypeDtoId = getFinancialOperation.OperationTypeId
        };
    }
}