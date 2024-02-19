using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Get;

public record GetFinancialOperationQuery(int FinancialOperationId) :  IRequest<FinancialOperationDto>;


