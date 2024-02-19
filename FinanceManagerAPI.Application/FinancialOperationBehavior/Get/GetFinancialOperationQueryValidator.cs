using FluentValidation;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Get;

public class GetFinancialOperationQueryValidator : AbstractValidator<GetFinancialOperationQuery>
{
    public GetFinancialOperationQueryValidator()
    {
        RuleFor(query => query.FinancialOperationId)
            .NotNull().WithMessage("Error: Value [Id] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Id] shouldn't be empty or 0.");
    }
}