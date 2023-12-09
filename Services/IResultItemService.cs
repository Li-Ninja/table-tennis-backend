using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Dtos.ResultItem;

namespace table_tennis_backend.Services;

public interface IResultItemService
{
    Task<string> AddResultItem(AddReqDto[] req);
    Task<List<GetResDto>> GetAllResultItemList();
    Task<List<GetResDto>> GetResultItemList(int id);
    // Task UpdateResultItem(UpdateReqDto req);
}
