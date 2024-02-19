using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Create;

public record CreateFinancialOperationCommand(FinancialOperationDto FinancialOperationDto) : IRequest<Unit> { }