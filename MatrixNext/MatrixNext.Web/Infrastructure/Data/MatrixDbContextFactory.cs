using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MatrixNext.Web.Infrastructure.Data
{
    /// <summary>
    /// Design-time factory for creating DbContext instances
    /// Used by EF Core CLI tools (migrations, etc) at design time
    /// </summary>
    public class MatrixDbContextFactory : IDesignTimeDbContextFactory<MatrixDbContext>
    {
        public MatrixDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MatrixDbContext>();
            
            // Read connection string from environment or appsettings.json
            var connectionString = "Server=.;Database=Matrix_EasyQuote;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";
            
            optionsBuilder.UseSqlServer(connectionString);

            return new MatrixDbContext(optionsBuilder.Options);
        }
    }
}
