using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class PlayerScoreHistoryRepository : IPlayerScoreHistoryRepository
{
    private readonly TableTennisContext _db;

    public PlayerScoreHistoryRepository(TableTennisContext db)
    {
        _db = db;
    }

    public async Task CreatePlayerScore(IEnumerable<PlayerScoreHistory> score)
    {
        _db.PlayerScoreHistory.AddRange(score);
        await _db.SaveChangesAsync();
    }

    public async Task<List<PlayerScoreHistory>> CreatePlayerScore()
    {
        return await _db.PlayerScoreHistory.ToListAsync();
    }
}
