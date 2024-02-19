using FinanceManagerAPI.Domain.Models;
using MediatR;

namespace FinanceManagerAPI.Application.ReportBehavior.DailyReport;

public record GetDailyReportQuery(DateTime InputDate) : IRequest<DailyReportResponse> { }

public record DailyReportResponse(
    decimal TotalIncome,
    decimal TotalExpenses,
    List<FinancialOperation> Operations
    );