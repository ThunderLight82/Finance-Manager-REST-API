using FluentValidation;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Create;

public class CreateOperationTypeCommandValidator : AbstractValidator<CreateOperationTypeCommand>
{
    public CreateOperationTypeCommandValidator()
    {
        RuleFor(command => command.OperationTypeDto.Name)
            .NotNull().WithMessage("Error: Value [Name] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Name] shouldn't be empty or whitespace.");
        
        RuleFor(command => command.OperationTypeDto.IsIncomeOperation)
            .NotNull().WithMessage("Error: Value [IsIncomeOperation] shouldn't be null.")
            .IsInEnum().WithMessage("Error: Value [IsIncomeOperation] should be a valid boolean.");
    }
}