using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;

namespace FinanceManagerAPI.Application.OperationTypeBehavior;

internal class OperationTypeBehavior : BaseBehavior<OperationType>
{
    public OperationTypeBehavior(FinanceAPIDbContext dbContext) : base(dbContext) { }
}