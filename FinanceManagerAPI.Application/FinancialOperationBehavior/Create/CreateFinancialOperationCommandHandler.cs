using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Create;

internal class CreateFinancialOperationCommandHandler : IRequestHandler<CreateFinancialOperationCommand>
{
    private readonly IBaseBehavior<FinancialOperation> _financialOperationBehavior;
    private readonly ILogger<CreateFinancialOperationCommandHandler> _logger;

    public CreateFinancialOperationCommandHandler(IBaseBehavior<FinancialOperation> financialOperationBehavior, ILogger<CreateFinancialOperationCommandHandler> logger)
    {
        _financialOperationBehavior = financialOperationBehavior;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateFinancialOperationCommand request, CancellationToken cancellationToken)
    {
        var financialOperationDto = request.FinancialOperationDto;
        
        // Вот тут я не совсем уверен какие поля должны создаваться вместе с новой Фин.операцией. (Это же применимо к некоторым другим хэндлерам) 
        // Как можно создавать Фин.операцию без типа операции указаной изначально? Айди всегда будет нулл ибо ТипаОперации не существует еще.
        // Или мне буквально нужно создавать две модели в одном хэндлере чтобы это выглядило логично? Фин.Опер + Тип Опер.
        // В задании указано так будто это 2 отдельные фичи и метод "создать" должен быть и у Типов и у Фин.Операции отдельно,
        // но опять же как Тип может существовать без Фин.Оп - не ясно.
        // Не могу никак понять как они должны "конектиться" между собой при методе создания или изменения.
        var newFinancialOperation = new FinancialOperation
        {
            DateTime = financialOperationDto.DateTime,
            Amount = financialOperationDto.Amount,
            OperationTypeId = financialOperationDto.OperationTypeDtoId,
            
            //Может нужно добавить еще что-то по типу этого сюда дополнительно
            //OperationType = financialOperationDto.OperationTypeDto
        };

        await _financialOperationBehavior.Create(newFinancialOperation);

        await _financialOperationBehavior.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Financial Operation with Id [{newFinancialOperation.Id}] was successfully created.");
        return Unit.Value;
    }
}