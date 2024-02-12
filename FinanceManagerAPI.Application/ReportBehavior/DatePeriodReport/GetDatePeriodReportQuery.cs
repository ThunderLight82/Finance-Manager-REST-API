using FinanceManagerAPI.Domain.Models;
using MediatR;

namespace FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;

public record GetDatePeriodReportQuery(DateTime StartInputDate, DateTime EndInputDate) : IRequest<DatePeriodReportResponse> { }

public record DatePeriodReportResponse(
    decimal TotalIncome,
    decimal TotalExpenses,
    List<FinancialOperation> Operations
    );