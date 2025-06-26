using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;
    public class StudentService:GenericService<Student>
    {
        public StudentService(IHostEnvironment env) : base( "StudentData.json")
        {
        
        }

   public Student  Create(Student student)
    {
        student.HashPassword = PasswordService.HashPassword(student.HashPassword);
        return base.Create(student);
    }
}
