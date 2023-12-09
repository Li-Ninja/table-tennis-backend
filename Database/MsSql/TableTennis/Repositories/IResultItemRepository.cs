using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Database.MsSql.TableTennis.Repositories;
public interface IResultItemRepository
{
    Task CreateResultItem(IEnumerable<ResultItem> result);

    Task<List<ResultItem>> FindResultItemById(int id);
    Task UpdateResultItem(ResultItem result);
    Task DeleteResultItem(List<int> ids);
}
