
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using myProject.Controllers;
using myProject.Models;
using myProject.Servise;
namespace myProject.Services;


public class LoginController : ControllerBase
{
    // פעולה להציג את עמוד הלוגין
    private LoginService loginService;
    private HttpContext httpContext;
    public LoginController(LoginService service){
        this.loginService = service;
    }
    

    // פעולה לאימות המשתמש
    [HttpPost]
    public IActionResult Login(User user)
    {
        if (loginService.Login(user, httpContext))
        {
            
            return RedirectToAction("Index", "Home");
        }
        return NotFound(); // החזר את העמוד אם האימות נכשל
    }
}