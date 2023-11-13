using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Player;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _service;

    public PlayerController(IPlayerService service)
    {
        _service = service;
    }

    // GET: api/Player
    [HttpGet]
    // TODO result and generic
    public async Task<IEnumerable<Player>> GetAllPlayerList()
    {
        return await _service.GetAllPlayer();
    }

    // POST: api/Player
    [HttpPost]
    public async Task<string> Create(AddReqDto[] req)
    {
        await _service.AddPlayer(req);

        return "success";
    }


    // PUT: api/Player/5
    [HttpPut("{id}")]
    public async Task<string> UpdatePlayer(UpdateReqDto req)
    {
        await _service.UpdatePlayer(req);

        return "success";
    }
}

