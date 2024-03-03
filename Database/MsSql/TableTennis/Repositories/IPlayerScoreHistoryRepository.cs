using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Database.MsSql.TableTennis.Repositories;
public interface IPlayerScoreHistoryRepository
{
    Task CreatePlayerScore(IEnumerable<PlayerScoreHistory> score);
    Task<List<PlayerScoreHistory>> CreatePlayerScore();
}
