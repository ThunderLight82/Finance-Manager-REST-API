using FluentValidation;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Update;

public class UpdateFinancialOperationCommandValidator : AbstractValidator<UpdateFinancialOperationCommand>
{
    public UpdateFinancialOperationCommandValidator()
    {
        RuleFor(command => command.FinancialOperationDto.Id)
            .NotNull().WithMessage("Error: Value [Id] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Id] shouldn't be empty or 0.");
        
        RuleFor(command => command.FinancialOperationDto.Amount)
            .NotNull().WithMessage("Error: Value [Amount] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Amount] shouldn't be empty or 0.")
            .GreaterThanOrEqualTo(0).WithMessage("Error: Value [Amount] should be greater that 0.");

        RuleFor(command => command.FinancialOperationDto.DateTime)
            .NotNull().WithMessage("Error: Value [DateTime] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [DateTime] shouldn't be empty.");

        RuleFor(command => command.FinancialOperationDto.OperationTypeDtoId)
            .NotNull().WithMessage("Error: Value [Type] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Type] shouldn't be empty or 0.");
    }
}