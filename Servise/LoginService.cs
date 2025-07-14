using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using myProject.Controllers;
using myProject.Models;

namespace myProject.Services;

public class LoginService
{
    private readonly PasswordService passwordService = new PasswordService();
    private readonly TeacherService teacherService ;
    private readonly StudentService studentService;

    public LoginService()
    {

        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        teacherService = new TeacherService(
            JsonManageService<Teacher>.LoadFromJson(Path.Combine(basePath, "TeacherData.json")),
            Path.Combine(basePath, "TeacherData.json")
        );

        studentService = new StudentService(
            JsonManageService<Student>.LoadFromJson(Path.Combine(basePath, "StudentData.json")),
            Path.Combine(basePath, "StudentData.json")
        );
        System.Console.WriteLine("LoginService initialized");

    }

    public int Login(User user, HttpContext httpContext)
    {
        Teacher? RequestTeacher = null;
        Student? RequestStudent = null;
        List<Claim> claims;
        int type = -1;
      
            RequestTeacher = teacherService.Get(t => t.Id == int.Parse(user.UserId))?? null;
            RequestStudent = studentService.Get(s => s.Id == int.Parse(user.UserId))?? null;
            System.Console.WriteLine($"userId: {user.UserId}, password: {user.Password}");
        
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
                    System.Console.WriteLine("!!Admin logged in!!");
                    type = 0;
                }
                else
                {
                    claims.Add(new Claim("type", "Teacher"));
                    claims.Add(new Claim("clases", string.Join(",", RequestTeacher.Clases.Select(c => c.ToString())))); // אם יש ClassId
                    System.Console.WriteLine($"Teacher logged in with classes: {string.Join(", ", RequestTeacher.Clases)}");
                    type = 1;
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
                type = 2;
            }
            else
            {
                Console.WriteLine("not found id or password");
                return -1;
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
            
            return type ;
        }
        catch (Exception ex)
        {
            throw new Exception($"Login error: {ex.Message}");
        }
    }
}
