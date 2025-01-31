using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Const;
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
    public async Task<IActionResult> GetAllPlayerList()
    {
        var players = await _service.GetAllPlayer();
        return Ok(players);
    }

    // GET: api/Player/DoublePlayer
    [HttpGet("DoublePlayer")]
    public async Task<IActionResult> GetAllDoublePlayerList()
    {
        var players = await _service.GetAllDoublePlayer();
        return Ok(players);
    }

    // GET: api/Player/comparison
    [HttpGet("Comparison")]
    // TODO result and generic
    public async Task<IActionResult> GetPlayerComparison([FromQuery] GetComparisonReqDto req)
    {
        var result = await _service.GetComparison(req);

        return Ok(result);
    }

    // POST: api/Player
    [HttpPost]
    public async Task<IActionResult> Create(AddReqDto[] req)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.AddPlayer(req);

        return Ok("success");
    }


    // PUT: api/Player/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(UpdateReqDto req)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.UpdatePlayer(req);

        return Ok("success");
    }
}

