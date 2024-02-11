using FinanceManagerAPI.Application.Interfaces;
using FinanceManagerAPI.DataAccess;
using FinanceManagerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerAPI.Application;

internal class BaseBehavior<TBaseModel> : IBaseBehavior<TBaseModel> where TBaseModel : BaseModel
{
    private readonly FinanceAPIDbContext _dbContext;

    protected BaseBehavior(FinanceAPIDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public virtual async Task<TBaseModel> GetById(int id) =>
        (await _dbContext.Set<TBaseModel>().FindAsync(id))!;

    public virtual async Task<IEnumerable<TBaseModel>> GetAll() =>
        await _dbContext.Set<TBaseModel>().ToListAsync();

    public virtual async Task Create(TBaseModel model) => 
        await _dbContext.Set<TBaseModel>().AddAsync(model);

    public virtual Task Delete(TBaseModel model)
    {
        _dbContext.Set<TBaseModel>().Remove(model);

        return Task.CompletedTask;
    }

    public virtual Task Update(TBaseModel model)
    {
        _dbContext.Entry(model).State = EntityState.Modified;
        
        return Task.CompletedTask;
    }

    //UnitOfWork SaveChanges is declared here instead of in DataAccess layer.
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken) => 
        await _dbContext.SaveChangesAsync(cancellationToken);
}