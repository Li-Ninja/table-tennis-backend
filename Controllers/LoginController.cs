using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using table_tennis_backend.Dtos.Login;

namespace table_tennis_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LoginController : ControllerBase
{


    // POST: api/Login

    [HttpPost]
    public IActionResult Create(AddReqDto req)
    {
        if (!(req.Account == "admin" && req.Password == "admin5566"))
        {
            HttpContext.Response.StatusCode = 401;
            return new JsonResult("failed");
        }

        var obj = new
        {
            res = "success",
            token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkZha2VVc2VyIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE2MjYyMzkwMjJ9.mF_sZUO3W5eKpKfOqhPDwzG5M5eNqR6Sb4wZaM0Y6Mg;"
        };

        return new JsonResult(obj);
    }

}

