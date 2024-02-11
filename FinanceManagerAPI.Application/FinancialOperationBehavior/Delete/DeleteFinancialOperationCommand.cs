using FinanceManagerAPI.DTO.ModelsDTOs;
using MediatR;

namespace FinanceManagerAPI.Application.FinancialOperationBehavior.Delete;

public record DeleteFinancialOperationCommand(FinancialOperationDto FinancialOperationDto) : IRequest<Unit> { }
