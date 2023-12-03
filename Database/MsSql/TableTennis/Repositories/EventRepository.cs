using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class EventRepository : IEventRepository
{
    private readonly TableTennisContext _db;

    public EventRepository(TableTennisContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Event>> ReadAllEvent()
    {
        return await _db.Event.ToListAsync();
    }
}
