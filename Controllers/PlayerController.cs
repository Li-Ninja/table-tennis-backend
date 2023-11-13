using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlayerController : ControllerBase
{
    private readonly TableTennisContext _context;

    public PlayerController(TableTennisContext context)
    {
        _context = context;
    }

    // GET: api/Player
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayer()
    {
        return await _context.Player.ToListAsync();
    }

    // GET: api/Player/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _context.Player.FindAsync(id);

        if (player == null)
        {
            return NotFound();
        }

        return player;
    }

    // POST: api/Player
    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(Player player)
    {
        _context.Player.Add(player);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }

    // PUT: api/Player/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, Player player)
    {
        if (id != player.Id)
        {
            return BadRequest();
        }

        _context.Entry(player).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PlayerExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Player/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        var player = await _context.Player.FindAsync(id);
        if (player == null)
        {
            return NotFound();
        }

        _context.Player.Remove(player);
        await _context.SaveChangesAsync();

        return NoContent();
    }

     private bool PlayerExists(int id)
    {
        return _context.Player.Any(e => e.Id == id);
    }
}

