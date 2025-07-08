
using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;
using System.Security.Claims;

namespace myProject.Services;

public class UserContextService
{
    private readonly HttpContext _httpContext;

    public UserContextService(IHttpContextAccessor accessor)
    {
        _httpContext = accessor.HttpContext!;
    }

    public string? UserId => _httpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value;

    public string? Type => _httpContext.User.FindFirst("type")?.Value;

    public string? Clas => _httpContext.User.FindFirst("clas")?.Value;

    public List<string> Clases =>
        _httpContext.User.FindFirst("clases")?.Value?.Split(',').ToList() ?? new();
}
