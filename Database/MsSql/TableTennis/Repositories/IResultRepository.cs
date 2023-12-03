using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Database.MsSql.TableTennis.Repositories;
public interface IResultRepository
{
    Task CreateResult(IEnumerable<Result> result);
    Task<IEnumerable<Result>> ReadAllResult();

    Task<Result?> FindResultById(int id);
    Task UpdateResult(Result result);
}
