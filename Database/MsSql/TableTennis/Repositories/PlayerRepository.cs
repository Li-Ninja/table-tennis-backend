using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class PlayerRepository : IPlayerRepository
{
    private readonly TableTennisContext _db;

    public PlayerRepository(TableTennisContext db)
    {
        _db = db;
    }

    public async Task CreatePlayer(IEnumerable<Player> players)
    {
        _db.Player.AddRange(players);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Player>> ReadAllPlayer()
    {
        return await _db.Player.ToListAsync();
    }

    public async Task<Player?> FindPlayerById(int id)
    {
        return await _db.Player.FindAsync(id);
    }

    public async Task UpdatePlayer(Player player)
    {
        _db.Player.Update(player);
        await _db.SaveChangesAsync();
    }

    public async Task UpdatePlayers(List<Player> playersToUpdate)
    {
        foreach (var player in playersToUpdate)
        {
            _db.Entry(player).State = EntityState.Modified;
        }
        await _db.SaveChangesAsync();
    }

}
