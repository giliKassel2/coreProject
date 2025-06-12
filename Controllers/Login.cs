
using System.Security.Claims;
using myProject.Controllers;
using myProject.Models;
using myProject.Servise;
namespace myProject.Services;
public class LoginService{
    PasswordService passwordService = new PasswordService();

    public bool Login(User user, HttpContext httpContext)
    {
        List<Claim>claims ;
        Teacher? RequestTeacher = TeacherService.Get(int.Parse(user.UserId));
        Student? RequestStudent = StudentService.Get(int.Parse(user.UserId)) ;

        if(RequestTeacher !=null && passwordService.VerifyPassword(user.Password,RequestTeacher.HashPassword)){
            claims =new List<Claim>();
            {
                new Claim(ClaimTypes.PrimarySid,user.UserId );
            };
            if(RequestTeacher.Name == "admin"){
                claims.Add(
                    new Claim("type","principal"));
            }
            else{
                if(RequestTeacher.Name == "admin"){
                claims.Add(
                    new Claim("type","Teacher"));
                }
            }
        }
        else{
            if(RequestStudent !=null && passwordService.VerifyPassword(user.Password,RequestTeacher.HashPassword)){
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid,user.UserId ),
                    new Claim("type","Teacher")
                };
            }
            else{
                return false;
            }
    
        }
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
            
    
    private bool saveToken(Claim claim){
        return true;
    }
}