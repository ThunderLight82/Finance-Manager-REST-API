namespace FinanceManagerAPI.Domain.Models;

public class OperationType : BaseModel
{
    public string? Name { get; set; }
    
    public bool IsIncomeOperation { get; set; }
}