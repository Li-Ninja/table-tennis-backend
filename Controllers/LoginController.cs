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
    public async Task<string> Create(AddReqDto req)
    {

        if (!(req.Account == "admin" && req.Password == "admin5566"))
        {
            HttpContext.Response.StatusCode = 401;
            return "failed";
        }

        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, req.Account)
                    };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return "success";
    }

}

