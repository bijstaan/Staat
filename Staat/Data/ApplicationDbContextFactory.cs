using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Staat.Data
{
    public class ApplicationDbContextFactory : IDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("DataSource=app.db");
            optionsBuilder.UseMemoryCache(new MemoryCache(new MemoryCacheOptions()));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}