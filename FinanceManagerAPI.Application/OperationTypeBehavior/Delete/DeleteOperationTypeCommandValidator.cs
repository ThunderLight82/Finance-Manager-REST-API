using FluentValidation;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Delete;

public class DeleteOperationTypeCommandValidator : AbstractValidator<DeleteOperationTypeCommand>
{
    public DeleteOperationTypeCommandValidator()
    {
        RuleFor(command => command.OperationTypeDto.Id)
            .NotNull().WithMessage("Error: Value [Id] shouldn't be null.")
            .NotEmpty().WithMessage("Error: Value [Id] shouldn't be empty or 0.");
    }
}