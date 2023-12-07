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
    public async Task<IActionResult> GetAllResultList()
    {
        if (!Auth.ValidateToken(HttpContext))
        {
            HttpContext.Response.StatusCode = 403;
            return new JsonResult("");
        }

        var result = await _service.GetAllResult();

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
}

