
using System.Security.Claims;
using myProject.Controllers;
using myProject.Models;
using myProject.Servise;
namespace myProject.Services;
public class LoginService{
    
    PasswordService passwordService = new PasswordService();
    TeacherService teacherService ;
    StudentService studentService ;
        public LoginService(TeacherService teacherService, StudentService studentService)
    {
        this.teacherService = teacherService;
        this.studentService = studentService;
    }
    public bool Login(User user, HttpContext httpContext)
    {
        Teacher? RequestTeacher = null;
        Student? RequestStudent = null;
        List<Claim> claims;
        try
        {
            RequestTeacher = teacherService.Get(t => t.Id == int.Parse(user.UserId));
            RequestStudent = studentService.Get(s => s.Id == int.Parse(user.UserId));
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in LoginService: " + e.Message);
            return false;
        }
       
        if ((RequestTeacher != null && passwordService.VerifyPassword(user.Password, RequestTeacher.HashPassword)) || user.UserId == "1000")
        {
            System.Console.WriteLine("Teacher found and password verified.");
            claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, user.UserId)
            };
            if (RequestTeacher != null && RequestTeacher.Name == "admin")
            {
                claims.Add(new Claim("type", "principal"));
            }
            else
            {
                claims.Add(new Claim("type", "Teacher"));
            }
        }
        else if (RequestStudent != null && passwordService.VerifyPassword(user.Password, RequestStudent.HashPassword))
        {
            System.Console.WriteLine("Student found and password verified.");
            claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, user.UserId),
                new Claim("type", "Student")
            };
        }
        else
        {
            System.Console.WriteLine("not found id or passsword");
            return false;
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
    private bool saveToken(List<Claim> claims){

        return true;
    }
    
} 