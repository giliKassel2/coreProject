using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;

    public class TeacherService:GenericService<Teacher>{
    public TeacherService()
    {
    }

    public TeacherService(IHostEnvironment env) : base( "StudentData.json")
        {
            
        }
        
        public void Create(Teacher teacher)
        {
            teacher.HashPassword = PasswordService.HashPassword(teacher.HashPassword);
            base.Create(teacher);
        }
   
    }
