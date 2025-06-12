using myProject.Models;
namespace myProject.Interfaces;
public interface IClasService
{
    List<Student> Get();

    Student Get(int id);

    int Insert(Student newStudent);

    bool UpDate(int id , Student newStudent);

    bool Delete(int id);
}