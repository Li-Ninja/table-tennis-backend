using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Dtos.Result;

namespace table_tennis_backend.Services;

public interface IResultService
{
    Task AddResult(AddReqDto[] req);
    Task<GetResDto?> GetResult(GetReqDto req);
    Task<List<GetResDto>> GetAllResult();
    Task UpdateResult(UpdateReqDto req);
}
