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
                        .Include(r => r.PlayerA1)
                        .Include(r => r.PlayerA2)
                        .Include(r => r.PlayerB1)
                        .Include(r => r.PlayerB2)
                        .OrderByDescending(r => r.Round)
                        .ThenBy(r => r.RoundIndex)
                        .ToListAsync();
    }


    public async Task<Result?> FindResultById(int id)
    {
        return await _db.Result
                    .Include(r => r.Event)
                    .Include(r => r.PlayerA1)
                    .Include(r => r.PlayerA2)
                    .Include(r => r.PlayerB1)
                    .Include(r => r.PlayerB2)
                    .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Result?> FindResultByOtherId(int event_id, int round, int roundIndex)
    {
        return await _db.Result
                    .Include(r => r.Event)
                    .Include(r => r.PlayerA1)
                    .Include(r => r.PlayerA2)
                    .Include(r => r.PlayerB1)
                    .Include(r => r.PlayerB2)
                    .FirstOrDefaultAsync(r => r.Event_Id == event_id && r.Round == round && r.RoundIndex == roundIndex);
    }

    public async Task<IEnumerable<Result>> FindResultListByEventId(int event_id)
    {
        return await _db.Result.Where(r => r.Event_Id == event_id).ToListAsync();
    }

    public async Task UpdateResult(Result result)
    {
        var existingResult = await _db.Result.FindAsync(result.Id);

        if (existingResult != null)
        {
            existingResult.Player_Id_A_1 = result.Player_Id_A_1;
            existingResult.Player_Id_A_2 = result.Player_Id_A_2;
            existingResult.Player_Id_B_1 = result.Player_Id_B_1;
            existingResult.Player_Id_B_2 = result.Player_Id_B_2;
            existingResult.ScoreA = result.ScoreA ?? existingResult.ScoreA;
            existingResult.ScoreB = result.ScoreB ?? existingResult.ScoreB;

            await _db.SaveChangesAsync();
        }

    }

    public async Task DeleteResult(int id)
    {
        var result = await _db.Result.Where(r => r.Event_Id == id).ToListAsync();


        if (result != null)
        {
            _db.Result.RemoveRange(result);
            await _db.SaveChangesAsync();
        }
    }
}
