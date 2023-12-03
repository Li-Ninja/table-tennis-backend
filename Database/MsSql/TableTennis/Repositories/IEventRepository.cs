using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Database.MsSql.TableTennis.Repositories;
public interface IEventRepository
{
    Task<IEnumerable<Event>> ReadAllEvent();

}
