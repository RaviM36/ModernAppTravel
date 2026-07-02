using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoSultan.Wpf.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // Match the default connection used in appsettings.json
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AutoSultanDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        return new ApplicationDbContext(builder.Options);
    }
}
