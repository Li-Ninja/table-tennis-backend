using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Services;
using table_tennis_backend.Const;
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
    public async Task<IActionResult> GetAllResultList([FromQuery] GetAllReqDto req)
    {
        var result = await _service.GetAllResult(req);

        return Ok(result);
    }

    // POST: api/Result
    [HttpPost]
    public async Task<IActionResult> Create(AddReqDto[] req)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.AddResult(req);

        return Ok("success");

    }

    // POST: api/Result/ResultRanking
    [HttpPost("ResultRanking")]
    public async Task<IActionResult> CreateRanking(AddRankingReqDto[] req)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.AddResultRanking(req);

        return Ok("success");

    }


    // PUT: api/Result/
    [HttpPut]
    public async Task<IActionResult> UpdateResult(UpdateReqDto req)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }
        await _service.UpdateResult(req);

        return Ok("success");
    }

    // POST: api/Result
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResult(int id)
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        await _service.DeleteResult(id);

        return Ok("success");

    }
}

