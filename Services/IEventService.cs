using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Dtos.Event;

namespace table_tennis_backend.Services;

public interface IEventService
{
    Task<IEnumerable<GetAllResDto>> GetAllEvent();
}
