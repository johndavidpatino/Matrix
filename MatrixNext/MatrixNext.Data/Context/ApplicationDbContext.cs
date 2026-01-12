using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Data.Context
{
    /// <summary>
    /// Application DbContext - minimal implementation for Dapper-based adapters
    /// Note: This context is primarily used for database access via Dapper, not EF Core entities
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuration will be added as needed for EF Core features
            // For now, most data access uses Dapper directly via Database.GetDbConnection()
        }
    }
}
