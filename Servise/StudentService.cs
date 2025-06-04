using myProject.Models;
using myProject.Interfaces;
namespace myProject.Services;
    public class StudentService:GenericService<Student>
    {
        public StudentService(IHostEnvironment env) : base( "StudentData.json")
    {
        
    }
    //לוגיקה אם צריך 

    }
