using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Dtos.Player;
using GetDoublePlayerDto = table_tennis_backend.Dtos.DoublePlayer.GetAllResDto;
using UpdateDoublePlayerDto = table_tennis_backend.Dtos.DoublePlayer.UpdateReqDto;

namespace table_tennis_backend.Services;

public interface IPlayerService
{
    Task AddPlayer(AddReqDto[] req);
    Task<List<GetAllResDto>> GetAllPlayer();
    Task<List<GetDoublePlayerDto>> GetAllDoublePlayer();
    Task UpdatePlayer(UpdateReqDto req);
    Task<GetComparisonResDto> GetComparison(GetComparisonReqDto req);
}
