using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Dtos.Player;
using table_tennis_backend.Const;

namespace table_tennis_backend.Services;

public interface IScoreService
{
    Task<PlayerStats> GetPlayerStats(int idA, int idB, PlayerComparisonTypeEnum type);
}
