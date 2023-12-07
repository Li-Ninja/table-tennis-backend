using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class ResultItemRepository : IResultItemRepository
{
    private readonly TableTennisContext _db;

    public ResultItemRepository(TableTennisContext db)
    {
        _db = db;
    }

    public async Task CreateResultItem(IEnumerable<ResultItem> result)
    {
        _db.ResultItem.AddRange(result);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ResultItem>> FindResultItemById(int id)
    {
        return await _db.ResultItem
                    .Where(ri => ri.Result_Id == id) // 假设您是根据 ResultId 来查找
                    .ToListAsync();
    }

    public async Task UpdateResultItem(ResultItem result)
    {
        _db.ResultItem.Update(result);
        await _db.SaveChangesAsync();
    }
}