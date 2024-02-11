namespace FinanceManagerAPI.Domain.Models;

public class FinancialOperation : BaseModel
{
    public DateTime DateTime { get; set; }

    public decimal Amount { get; set; }

    public int OperationTypeId { get; set; }
    
    public OperationType? OperationType { get; set; }
}