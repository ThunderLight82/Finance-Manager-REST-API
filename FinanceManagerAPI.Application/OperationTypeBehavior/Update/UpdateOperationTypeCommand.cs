using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Update;

public record UpdateOperationTypeCommand(OperationTypeDto OperationTypeDto) : IRequest<Unit> { }
