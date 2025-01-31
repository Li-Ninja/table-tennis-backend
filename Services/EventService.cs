
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Event;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;


public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetAllResDto>> GetAllEvent()
    {
        var events = await _repository.ReadAllEvent();
        return events.Select(e => new GetAllResDto
        {
            Id = e.Id,
            Name = e.Name,
            Date = e.Date,
            Type = e.Type,
        }).ToList();
    }

}

