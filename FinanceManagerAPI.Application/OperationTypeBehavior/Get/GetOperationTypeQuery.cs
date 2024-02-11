using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.OperationTypeBehavior.Get;

public record GetOperationTypeQuery(int OperationTypeId) :  IRequest<OperationTypeDto>;


