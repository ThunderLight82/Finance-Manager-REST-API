namespace FinanceManagerAPI.DTO.ModelsDTOs;

public class OperationTypeDto : BaseModelDto
{ 
    public string? Name { get; init; }
    
    public bool IsIncomeOperation { get; init; }
}