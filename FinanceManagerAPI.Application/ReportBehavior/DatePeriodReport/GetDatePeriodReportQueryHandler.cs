using FinanceManagerAPI.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;

internal class GetDatePeriodReportQueryHandler : IRequestHandler<GetDatePeriodReportQuery, DatePeriodResponse>
{
    private readonly FinanceAPIDbContext _dbContext;
    private readonly ILogger<GetDatePeriodReportQueryHandler> _logger;

    public GetDatePeriodReportQueryHandler(FinanceAPIDbContext dbContext, ILogger<GetDatePeriodReportQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<DatePeriodResponse> Handle(GetDatePeriodReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var financialOperations = await _dbContext.FinancialOperations
                .Where(fo => fo.DateTime >= request.StartInputDate && fo.DateTime <= request.EndInputDate)
                .Include(fo => fo.OperationType)
                .ToListAsync(cancellationToken);
            
            if (financialOperations == null)
            {
                _logger.LogError("Error in  GetDatePeriodReportQueryHandler." +
                                 "Financial operations with this date not found.");
                throw new Exception("Financial operations with this date not found.");
            }
            
            decimal totalIncome = financialOperations
                .Where(fo => fo.OperationType!.IsIncomeOperation)
                .Sum(fo => fo.Amount);
        
            decimal totalExpenses = financialOperations
                .Where(fo => !fo.OperationType!.IsIncomeOperation)
                .Sum(fo => fo.Amount);
            
            return new DatePeriodResponse(totalIncome, totalExpenses, financialOperations);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while fetching date period report: {ex.Message}");
            throw;
        }
    }
}