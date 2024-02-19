using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Update;

public record UpdateFinancialOperationCommand(FinancialOperationDto FinancialOperationDto) : IRequest<Unit> { }

