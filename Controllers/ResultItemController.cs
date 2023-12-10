using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
using table_tennis_backend.Const;
using table_tennis_backend.Dtos.ResultItem;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Database.MsSql.TableTennis.Model;

namespace table_tennis_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ResultItemController : ControllerBase
{
    private readonly IResultItemService _service;

    public ResultItemController(IResultItemService service)
    {
        _service = service;
    }

    // GET: api/ResultItem
    [HttpGet]
    public async Task<IActionResult> GetAllResultItemList()
    {
        var result = await _service.GetAllResultItemList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    // TODO result and generic
    public async Task<IActionResult> GetResultItemList(int id)
    {
        var result = await _service.GetResultItemList(id);

        return Ok(result);
    }

    // POST: api/ResultItem
    [HttpPost]
    public async Task<IActionResult> Create(AddReqDto[] req)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.AddResultItem(req);

        return Ok("success");
    }

    // DELETE: api/ResultItem
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResultItem(int id)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.DeleteResultItem(id);

        return Ok("success");
    }
}

