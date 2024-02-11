using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Delete;

public record DeleteOperationTypeCommand(OperationTypeDto OperationTypeDto) : IRequest<Unit> { }
