using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Database.MsSql.TableTennis.Repositories;
public interface IDoublePlayerRepository
{
    Task<List<DoublePlayer>> ReadAllDoublePlayer();
    Task UpdateDoublePlayers(List<DoublePlayer> playersToUpdate);
    Task<DoublePlayer?> FindDoublePlayerById(int id);
}
