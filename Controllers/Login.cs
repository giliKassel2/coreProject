
using Microsoft.AspNetCore.Mvc;
using myProject.Models;
using myProject.Services;
using myProject.Interfaces;

namespace myProject.Controllers;

[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private LoginService loginService;
    public LoginController(LoginService service)
    {
        this.loginService = service;
    }


    // פעולה לאימות המשתמש
    [HttpPost]
    public IActionResult Login([FromBody] User user)
    {
        System.Console.WriteLine("start login post----");
        //UserContextService userContext = new UserContextService( a);
        var t = loginService.Login(user, HttpContext);
        if (t>=0)
        {
            //  return Redirect("\\index.html");
            var token = HttpContext.Response.Headers["Set-Cookie"];
            return Ok(new { success = true, cookie = token, type = t });

        }
        return NotFound(); // החזר את העמוד אם האימות נכשל
    }
}