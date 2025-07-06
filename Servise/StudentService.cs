using myProject.Models;
using myProject.Interfaces;
using myProject.Controllers;

namespace myProject.Services;
    public class StudentService:GenericService<Student>
    {
        // List<Student> students = new List<Student>();
        // JsonManageService<Student> jsonFileService;
        public StudentService(IHostEnvironment env) :base(
             JsonManageService<Student>.LoadFromJson(
                Path.Combine(env.ContentRootPath, "Data", "students.json")
            ),Path.Combine(env.ContentRootPath, "Data", "students.json")
        )
        {
           
        }

   public Student  Create(Student student)
    {
        student.HashPassword = PasswordService.HashPassword(student.HashPassword);
        return base.Create(student);
    }
}
