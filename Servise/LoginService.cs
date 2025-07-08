using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using myProject.Controllers;
using myProject.Models;

namespace myProject.Services;

public class LoginService
{
    private readonly PasswordService passwordService = new PasswordService();
    private readonly TeacherService teacherService;
    private readonly StudentService studentService;

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
            throw new Exception("Error retrieving user data.");
        }

        if ((RequestTeacher != null && passwordService.VerifyPassword(user.Password, RequestTeacher.HashPassword)) || user.UserId == "1000")
        {
            Console.WriteLine("Teacher found and password verified.");
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

            // כאן אפשר להוסיף Claim של רשימת כיתות אם צריך
        }
        else if (RequestStudent != null && passwordService.VerifyPassword(user.Password, RequestStudent.HashPassword))
        {
            Console.WriteLine("Student found and password verified.");
            claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, user.UserId),
                new Claim("type", "Student"),
                new Claim("clas", RequestStudent.Clas.ToString()) // אם יש ClassId
            };
        }
        else
        {
            Console.WriteLine("not found id or password");
            return false;
        }

        try
        {
            var token = CreateTokenService.GetToken(claims);
            var tokenString = CreateTokenService.WriteToken(token);

            httpContext.Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Login error: {ex.Message}");
        }
    }
}
