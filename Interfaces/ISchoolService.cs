using myProject.Models;
using myProject.Models;
namespace myProject.Interfaces;
public interface ISchoolService
{
    List<Teacher> GetTeachers();
    List<Student> GetStudents();
    Student GetStudent(int id);
    Teacher GetTeacher(int id);
    int InsertTeacher(Teacher t);
    int InsertStudent(int id,String name);
    bool UpdateStudent(int id , Student s);
    bool UpdateTeacher(int id , Teacher t);
    bool DeleteTeacher(int id);
    bool DeleteStudent(int id);

}