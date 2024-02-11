using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Get;

internal class GetOperationTypeQueryHandler : IRequestHandler<GetOperationTypeQuery, OperationTypeDto>
{
    private readonly FinanceAPIDbContext _dbContext;
    private readonly ILogger<GetOperationTypeQueryHandler> _logger;

    public GetOperationTypeQueryHandler(FinanceAPIDbContext dbContext, ILogger<GetOperationTypeQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<OperationTypeDto> Handle(GetOperationTypeQuery request, CancellationToken cancellationToken)
    {
        var operationTypeId = request.OperationTypeId;

        var getOperationType = await _dbContext.OperationsTypes
            .Where(ot => ot.Id == operationTypeId)
            .FirstOrDefaultAsync(cancellationToken);
            
        if (getOperationType == null)
        {
            _logger.LogError($"Error in OperationTypeBehavior - Get." +
                             $"Operation type with ID {request.OperationTypeId} not found.");
            throw new Exception($"Operation type with ID {request.OperationTypeId} not found.");
        }

        return new OperationTypeDto
        {
            Id = getOperationType.Id,
            Name = getOperationType.Name,
            IsIncomeOperation = getOperationType.IsIncomeOperation
        };
    }
}