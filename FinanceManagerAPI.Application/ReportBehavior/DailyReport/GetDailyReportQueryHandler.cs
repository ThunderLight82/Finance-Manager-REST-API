using FinanceManagerAPI.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.ReportBehavior.DailyReport;

internal class GetDailyReportQueryHandler : IRequestHandler<GetDailyReportQuery, DailyReportResponse>
{
    private readonly FinanceAPIDbContext _dbContext;
    private readonly ILogger<GetDailyReportQueryHandler> _logger;

    public GetDailyReportQueryHandler(FinanceAPIDbContext dbContext, ILogger<GetDailyReportQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<DailyReportResponse> Handle(GetDailyReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var financialOperations = await _dbContext.FinancialOperations
                .Where(fo => fo.DateTime.Date == request.InputDate.Date)
                .Include(fo => fo.OperationType)
                .ToListAsync(cancellationToken);

            if (financialOperations.Count == 0)
            {
                _logger.LogError("Error in  GetDailyReportQueryHandler." +
                                 "Financial operation with this date not found.");
                throw new InvalidOperationException($"Financial operation with this date '{request.InputDate}' not found.");
            }
            
            decimal totalIncome = financialOperations
                .Where(fo => fo.OperationType!.IsIncomeOperation)
                .Sum(fo => fo.Amount);
        
            decimal totalExpenses = financialOperations
                .Where(fo => !fo.OperationType!.IsIncomeOperation)
                .Sum(fo => fo.Amount);
            
            return new DailyReportResponse(totalIncome, totalExpenses, financialOperations);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while generating daily report: {ex.Message}");
            throw;
        }
    }
}