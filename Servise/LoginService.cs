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

    public LoginService()
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        try
        {
            var teachers = JsonManageService<Teacher>.LoadFromJson(Path.Combine(basePath, "TeacherData.json"));
            teacherService = new TeacherService(teachers, Path.Combine(basePath, "TeacherData.json"));
        }
        catch
        {
            teacherService = new TeacherService(new List<Teacher>(), Path.Combine(basePath, "TeacherData.json"));
        }

        try
        {
            var students = JsonManageService<Student>.LoadFromJson(Path.Combine(basePath, "StudentData.json"));
            studentService = new StudentService(students, Path.Combine(basePath, "StudentData.json"));
        }
        catch
        {
            studentService = new StudentService(new List<Student>(), Path.Combine(basePath, "StudentData.json"));
        }
    }

    public int Login(User user, HttpContext httpContext)
    {
        if (!int.TryParse(user.UserId, out int userIdInt))
            return -1;

        Teacher? teacher = null;
        Student? student = null;
        List<Claim> claims = new();
        int type = -1;

        try
        {
            // אם המשתמש הוא ID 1000 - דלג על בדיקת סיסמה, המשך ישירות לבדוק אם הוא מורה
            if (userIdInt == 1000)
            {
                teacher = teacherService.Get(t => t.Id == 1000);
                if (teacher?.Name?.ToLower() == "admin")
                {
                    claims.Add(new Claim(ClaimTypes.PrimarySid, user.UserId));
                    claims.Add(new Claim("type", "principal"));
                    type = 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                teacher = teacherService.Get(t =>
                    t.Id == userIdInt &&
                    !string.IsNullOrWhiteSpace(t.HashPassword) &&
                    passwordService.VerifyPassword(user.Password, t.HashPassword)
                );

                if (teacher != null)
                {
                    claims.Add(new Claim(ClaimTypes.PrimarySid, user.UserId));
                    claims.Add(new Claim("type", "Teacher"));

                    if (teacher.Clases != null && teacher.Clases.Any())
                    {
                        claims.Add(new Claim("clases", string.Join(",", teacher.Clases)));
                    }

                    type = 1;
                }
                else
                {
                    student = studentService.Get(s =>
                        s.Id == userIdInt &&
                        !string.IsNullOrWhiteSpace(s.HashPassword) &&
                        passwordService.VerifyPassword(user.Password, s.HashPassword)
                    );

                    if (student != null)
                    {
                        claims.Add(new Claim(ClaimTypes.PrimarySid, user.UserId));
                        claims.Add(new Claim("type", "Student"));
                        claims.Add(new Claim("clas", student.Clas?.ToString() ?? ""));
                        type = 2;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        catch
        {
            return -1;
        }

        try
        {
            var token = CreateTokenService.GetToken(claims);
            var tokenString = CreateTokenService.WriteToken(token);

            httpContext.Response.Cookies.Append(
                "AuthToken",
                tokenString,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = httpContext.Request.IsHttps,
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                }
            );

            return type;
        }
        catch
        {
            return -1;
        }
    }
}
