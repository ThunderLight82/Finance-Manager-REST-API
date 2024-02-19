using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior;

internal class FinancialOperationBehavior : BaseBehavior<FinancialOperation>
{
    public FinancialOperationBehavior(FinanceAPIDbContext dbContext) : base(dbContext) { }
}