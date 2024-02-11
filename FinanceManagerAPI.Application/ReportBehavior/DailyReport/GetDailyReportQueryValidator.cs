using FluentValidation;

namespace FinanceManagerAPI.Application.ReportBehavior.DailyReport;

public class GetDailyReportQueryValidator : AbstractValidator<GetDailyReportQuery>
{
    public GetDailyReportQueryValidator()
    {
        RuleFor(query => query.InputDate)
            .NotNull().WithMessage("Error: Value [DateTime] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [DateTime] shouldn't be empty or 0.");
    }
}