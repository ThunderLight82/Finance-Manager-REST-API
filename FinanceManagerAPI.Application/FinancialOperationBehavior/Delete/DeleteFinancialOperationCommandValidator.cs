using FluentValidation;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Delete;

public class DeleteFinancialOperationCommandValidator : AbstractValidator<DeleteFinancialOperationCommand>
{
    public DeleteFinancialOperationCommandValidator()
    {
        RuleFor(command => command.FinancialOperationDto.Id)
            .NotNull().WithMessage("Error: Value [Id] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Id] shouldn't be empty or 0.");
    }
}