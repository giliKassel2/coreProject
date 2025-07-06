using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;

    public class TeacherService:GenericService<Teacher>{
    public TeacherService(IHostEnvironment env) : base(
         JsonManageService<Teacher>.LoadFromJson(
            Path.Combine(env.ContentRootPath, "Data", "teachers.json")
        ),Path.Combine(env.ContentRootPath, "Data", "teachers.json")
    )
        {
            
        }
        
        public Teacher Create(Teacher teacher)
        {
            teacher.HashPassword = PasswordService.HashPassword(teacher.HashPassword);
           return base.Create(teacher);
        }
   
    }
