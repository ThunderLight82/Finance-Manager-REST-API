namespace FinanceManagerAPI.DTO.ModelsDTOs;

public class FinancialOperationDto: BaseModelDto
{
    public DateTime DateTime { get; init; }
    
    public decimal Amount { get; init; }
    
    public int OperationTypeDtoId { get; init; }
    
    public virtual OperationTypeDto? OperationTypeDto { get; init; }
}