using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Result;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Database.MsSql.TableTennis.Model;

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
    [HttpGet]
    // TODO result and generic
    public async Task<IEnumerable<GetAllResDto>> GetAllResultList()
    {
        return await _service.GetAllResult();
    }

    // POST: api/Result
    [HttpPost]
    // public async Task<string> Create(AddReqDto[] req)
    // {
    //     await _service.AddResult(req);

    //     return "success";
    // }


    // PUT: api/Result/5
    [HttpPut("{id}")]
    public async Task<string> UpdateResult(UpdateReqDto req)
    {
        await _service.UpdateResult(req);

        return "success";
    }
}

