using FinanceManagerAPI.Domain.Models;
using MediatR;

namespace FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;

public record GetDatePeriodReportQuery(DateTime StartInputDate, DateTime EndInputDate) : IRequest<DatePeriodResponse> { }

public record DatePeriodResponse(
    decimal TotalIncome,
    decimal TotalExpenses,
    List<FinancialOperation> Operations
    );