using FinanceManagerAPI.Domain.Models;

namespace FinanceManagerAPI.Application.Interfaces;

public interface IBaseBehavior<TBaseModel> where TBaseModel : BaseModel
{
    Task<TBaseModel> GetById(int id);
    Task<IEnumerable<TBaseModel>> GetAll();
    Task Create(TBaseModel model);
    Task Delete(TBaseModel model);
    Task Update(TBaseModel model);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}