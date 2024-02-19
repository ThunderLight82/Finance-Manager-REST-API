using FinanceManagerAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerAPI.UnitTests;

public abstract class TestDbContext
{
    public static FinanceAPIDbContext CreateAndSeedTestDb()
    {
        var options = new DbContextOptionsBuilder<FinanceAPIDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemory_FinanceAPI_TestServer")
            .Options;
        
        var dbContext = new FinanceAPIDbContext(options);
        
        dbContext.SaveChangesAsync();

        return dbContext;
    }
}