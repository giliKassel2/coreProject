using System.Security.Claims;
using myProject.Models;
namespace myProject.Services;
public class LoginService{
   public bool Login(User user, HttpContext httpContext)
{
    //צריך להוסיף פה בפונקציה את הסיסמא המוצפנת ואת הסיסמא הלא מוצפנת זה מה שצריך לשלוח לו 
    if (user.Name == "admin" && VerifyPassword(user.Password, ))
    {
        // יצירת טוקן לאחר הצלחה בלוגין
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.PostalCode, ""),
            new Claim(ClaimTypes.Country, "principal")
        };
        var token = CreateTokenService.GetToken(claims);
        var tokenString = CreateTokenService.WriteToken(token);

        // הוספת הטוקן ל-Cookie
        httpContext.Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,   
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        });

        return true; 
    }
    
    return false; 
}
    private bool saveToken(Claim claim){

    }
}
