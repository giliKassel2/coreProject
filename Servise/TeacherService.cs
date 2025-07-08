using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;

    public class TeacherService:GenericService<Teacher>{
    
    public TeacherService(IHostEnvironment env) : base(
        GetFilteredTeachers(env, new UserContextService(new HttpContextAccessor()))
         , Path.Combine(env.ContentRootPath, "Data", "TeacherData.json")
    )
    {

    }
    public TeacherService(List<Teacher> teachers, string filePath)
         : base(teachers, filePath)
    {
        System.Console.WriteLine("TeacherService initialized");
    }
    
        private static List<Teacher> GetFilteredTeachers(IHostEnvironment env, UserContextService userContext)
    {
        var allTeachers = JsonManageService<Teacher>.LoadFromJson(
            Path.Combine(env.ContentRootPath, "Data", "TeacherData.json"));

        switch (userContext.Type)
        {
            case "principal":
                return allTeachers;

            case "Teacher":
                return allTeachers
                    .Where(t => t.Id.ToString() == userContext.UserId)
                    .ToList();
            default:
                return new List<Teacher>();
        }
    }
        public Teacher Create(Teacher teacher)
    {
        teacher.HashPassword = PasswordService.HashPassword(teacher.HashPassword);
        return base.Create(teacher);
    }
   
    }
