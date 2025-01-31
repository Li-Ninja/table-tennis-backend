using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class DoublePlayerRepository : IDoublePlayerRepository
{
    private readonly TableTennisContext _db;

    public DoublePlayerRepository(TableTennisContext db)
    {
        _db = db;
    }

    public async Task<List<DoublePlayer>> ReadAllDoublePlayer()
    {
        return await _db.DoublePlayer.ToListAsync();
    }

    public async Task UpdateDoublePlayers(List<DoublePlayer> playersToUpdate)
    {
        foreach (var player in playersToUpdate)
        {
            _db.Entry(player).State = EntityState.Modified;
        }
        await _db.SaveChangesAsync();
    }

    public async Task<DoublePlayer?> FindDoublePlayerById(int id)
    {
        return await _db.DoublePlayer.FindAsync(id);
    }

}
