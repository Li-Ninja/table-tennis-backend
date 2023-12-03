using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class ResultRepository : IResultRepository
{
    private readonly TableTennisContext _db;

    public ResultRepository(TableTennisContext db)
    {
        _db = db;
    }

    public async Task CreateResult(IEnumerable<Result> players)
    {
        _db.Result.AddRange(players);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Result>> ReadAllResult()
    {
        return await _db.Result
                          .Include(r => r.Event)
                         .ToListAsync();
    }


    public async Task<Result?> FindResultById(int id)
    {
        return await _db.Result.FindAsync(id);
    }

    public async Task UpdateResult(Result player)
    {
        _db.Result.Update(player);
        await _db.SaveChangesAsync();
    }
}
