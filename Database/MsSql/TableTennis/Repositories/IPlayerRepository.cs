using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Database.MsSql.TableTennis.Repositories;
public interface IPlayerRepository
{
    Task CreatePlayer(IEnumerable<Player> player);
    Task<List<Player>> ReadAllPlayer();
    Task<Player?> FindPlayerById(int id);
    Task UpdatePlayer(Player player);
    Task UpdatePlayers(List<Player> playersToUpdate);
}
