using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
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
    // TODO result and generic
    public async Task<IEnumerable<GetResDto>> GetResultItemList(GetReqDto req)
    {
        return await _service.GetResultItemList(req);
    }

    // POST: api/ResultItem
    [HttpPost]
    public async Task<string> Create(AddReqDto[] req)
    {
        await _service.AddResultItem(req);

        return "success";
    }
}

