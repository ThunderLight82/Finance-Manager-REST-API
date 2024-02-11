using FinanceManagerAPI.Application.OperationTypeBehavior.Delete;
using FluentValidation;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Update;

public class UpdateOperationTypeCommandValidator : AbstractValidator<UpdateOperationTypeCommand>
{
    public UpdateOperationTypeCommandValidator()
    {
        RuleFor(command => command.OperationTypeDto.Id)
            .NotNull().WithMessage("Error: Value [Id] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Id] shouldn't be empty or 0.");
        
        RuleFor(command => command.OperationTypeDto.Name)
            .NotNull().WithMessage("Error: Value [Name] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Name] shouldn't be empty or whitespace.");
        
        RuleFor(command => command.OperationTypeDto.IsIncomeOperation)
            .NotNull().WithMessage("Error: Value [IsIncomeOperation] shouldn't be null.")
            .IsInEnum().WithMessage("Error: Value [IsIncomeOperation] should be a valid boolean.");
    }
}