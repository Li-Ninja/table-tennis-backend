using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Event;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventController : ControllerBase
{
    private readonly IEventService _service;

    public EventController(IEventService service)
    {
        _service = service;
    }

    // GET: api/Event
    [HttpGet]
    // TODO result and generic
    public async Task<IEnumerable<GetAllResDto>> GetAllEventList()
    {
        return await _service.GetAllEvent();
    }
}

