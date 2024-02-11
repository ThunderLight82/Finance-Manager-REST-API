using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Create;

public record CreateOperationTypeCommand(OperationTypeDto OperationTypeDto) : IRequest<Unit> { }