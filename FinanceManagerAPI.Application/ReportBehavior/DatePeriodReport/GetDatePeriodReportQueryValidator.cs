using FluentValidation;

namespace FinanceManagerAPI.Application.ReportBehavior.DatePeriodReport;

public class GetDatePeriodReportQueryValidator : AbstractValidator<GetDatePeriodReportQuery>
{
    public GetDatePeriodReportQueryValidator()
    {
        RuleFor(query => query.StartInputDate)
            .NotNull().WithMessage("Error: Starting value [DateTime] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Starting value [DateTime] shouldn't be empty or 0.");
        
        RuleFor(query => query.EndInputDate)
            .NotNull().WithMessage("Error: Ending value [DateTime] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Ending value [DateTime] shouldn't be empty or 0.");
    }
}