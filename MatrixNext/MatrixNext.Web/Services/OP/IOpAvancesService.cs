using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

public interface IOpAvancesService
{
    Task<OpMigrationSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default);
}
