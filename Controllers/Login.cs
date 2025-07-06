
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
    public IActionResult Login(User user)
    {
        System.Console.WriteLine("start login post----");
        if (loginService.Login(user, HttpContext))
        {
            //  return Redirect("\\index.html");
            var token = HttpContext.Response.Headers["Set-Cookie"];
            return Ok(new { success = true, cookie = token });
            
        }
        return NotFound(); // החזר את העמוד אם האימות נכשל
    }
}