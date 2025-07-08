using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;
public class StudentService : GenericService<Student>
{
    public StudentService(IHostEnvironment env, IHttpContextAccessor accessor)
        : base(
            GetFilteredStudents(env, new UserContextService(accessor)),
            Path.Combine(env.ContentRootPath, "Data", "students.json")
        )
    {
    }

    private static List<Student> GetFilteredStudents(IHostEnvironment env, UserContextService userContext)
    {
        var allStudents = JsonManageService<Student>.LoadFromJson(
            Path.Combine(env.ContentRootPath, "Data", "students.json"));

        switch (userContext.Type)
        {
            case "principal":
                return allStudents;

            case "Teacher":
                return allStudents
                    .Where(s => userContext.Clases.Contains(s.Clas))
                    .ToList();

            case "Student":
                return allStudents
                    .Where(s => s.Id.ToString() == userContext.UserId)
                    .ToList();

            default:
                return new List<Student>();
        }
    }

    public Student Create(Student student)
    {
        student.HashPassword = PasswordService.HashPassword(student.HashPassword);
        return base.Create(student);
    }
}

