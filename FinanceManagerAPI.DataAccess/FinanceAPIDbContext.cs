using FinanceManagerAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerAPI.DataAccess;

public class FinanceAPIDbContext : DbContext
{
    public DbSet<FinancialOperation> FinancialOperations { get; set; } = null!;
    public DbSet<OperationType> OperationsTypes { get; set; } = null!;

    public FinanceAPIDbContext(DbContextOptions<FinanceAPIDbContext> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}