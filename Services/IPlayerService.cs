using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Dtos.Player;

namespace table_tennis_backend.Services;

public interface IPlayerService
{
    Task AddPlayer(AddReqDto[] req);
    Task<List<GetAllResDto>> GetAllPlayer();
    Task UpdatePlayer(UpdateReqDto req);
}
