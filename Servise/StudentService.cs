using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;
public class StudentService : GenericService<Student>
{
    public StudentService(IHostEnvironment env, IHttpContextAccessor accessor)
        : base(
            GetFilteredStudents(env, new UserContextService(accessor)),
            Path.Combine(env.ContentRootPath, "Data", "StudentData.json")
        )
    {
    }

    public StudentService(List<Student> students, string filePath)
        : base(students, filePath)
    {
    }
        
    private static List<Student> GetFilteredStudents(IHostEnvironment env, UserContextService userContext)
    {
        var allStudents = JsonManageService<Student>.LoadFromJson(
            Path.Combine(env.ContentRootPath, "Data", "StudentData.json"));

        switch (userContext.Type)
        {
            case "principal":
                return allStudents;

            case "Teacher":
                System.Console.WriteLine(allStudents.Where(s => userContext.Clases.Contains(s.Clas))
                        .ToList().Count);
                return allStudents.Where(s => userContext.Clases.Contains(s.Clas))
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

