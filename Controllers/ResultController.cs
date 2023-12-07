using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Result;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using Microsoft.AspNetCore.Authorization;

namespace table_tennis_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ResultController : ControllerBase
{
    private readonly IResultService _service;

    public ResultController(IResultService service)
    {
        _service = service;
    }

    // GET: api/Result
    [Authorize]
    [HttpGet]
    // TODO result and generic
    public async Task<List<GetResDto>> GetAllResultList()
    {
        return await _service.GetAllResult();
    }

    // POST: api/Result
    [Authorize]
    [HttpPost]
    public async Task<string> Create(AddReqDto[] req)
    {
        await _service.AddResult(req);

        return "success";
    }


    // PUT: api/Result/
    [Authorize]
    [HttpPut]
    public async Task<string> UpdateResult(UpdateReqDto req)
    {
        await _service.UpdateResult(req);

        return "success";
    }
}

