using FinanceManagerAPI.Application.FinancialOperationBehavior.Get;
using FluentValidation;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Get;

public class GetOperationTypeQueryValidator : AbstractValidator<GetOperationTypeQuery>
{
    public GetOperationTypeQueryValidator()
    {
        RuleFor(query => query.OperationTypeId)
            .NotNull().WithMessage("Error: Value [Id] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Id] shouldn't be empty or 0.");
    }
}