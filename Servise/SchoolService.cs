using myProject.Models;
using myProject.Controllers;
using myProject.Interfaces;
using System.Text.Json;
using myProject.Models;
namespace myProject.Servise;
public class SchoolService : ISchoolService
{
    private List<Student>? students;
    private List<Teacher>? teachers;
    private static string fileStudent = "students.json";
    private static string fileTeacher = "teachers.json";
    private string filePath;
     public SchoolService(IHostEnvironment environment)
    {
        filePath = Path.Combine(environment.ContentRootPath,"Data",fileStudent);
        
        using (var json = File.OpenText(filePath)){
            students= JsonSerializer.Deserialize<List<Student>>(json.ReadToEnd(),
             new JsonSerializerOptions
             {
                PropertyNameCaseInsensitive = true
            });
        }
        filePath = Path.Combine(environment.ContentRootPath,"Data",fileTeacher);
        using (var json = File.OpenText(filePath)){
            teachers= JsonSerializer.Deserialize<List<Teacher>>(json.ReadToEnd(),
             new JsonSerializerOptions
             {
                PropertyNameCaseInsensitive = true
            });
        }
    }
    private void saveStudentJson(){
        try{
           File.WriteAllText(filePath,JsonSerializer.Serialize(fileStudent)); 
        }
        catch (Exception ex){
        Console.WriteLine($"Error saving to JSON: {ex.Message}");
        }        
    }
    private void saveTeacherJson(){
        try{
           File.WriteAllText(filePath,JsonSerializer.Serialize(fileTeacher)); 
        }
        catch (Exception ex){
        Console.WriteLine($"Error saving to JSON: {ex.Message}");
        }        
    }
      private  bool isEmpty(Student s)
    {
        return s == null || string.IsNullOrWhiteSpace (s.Name);
    }
    public List<Teacher>? GetTeachers()
    {
        return teachers;
    }

    public List<Student>? GetStudents()
    {
        return students;
    }

    public Student? GetStudent(int id)
    {
        return students.First(s => s.Id == id);
    }
    public Teacher? GetTeacher(int id)
    {
        return teachers.FirstOrDefault(t => t.Id == id);
    }
    public int InsertTeacher(Teacher teacher)
    {
        if (teacher == null || string.IsNullOrWhiteSpace(teacher.Name))
        {
            return -1;
        }
        int maxId = teachers.Any() ? teachers.Max(t => t.Id) : 1000;
        teacher.Id = maxId + 1;
        this.teachers.Add(teacher);
        saveTeacherJson();
        return teacher.Id;
    }    
    public int InsertStudent(int id, String name)
    {
        Student s = new Student { Id = id, Name = name, presence = new List<Presence>() };
        int maxId = students.Any() ? students.Max( s => s.Id) : 100;
        s.Id = maxId+1;
        // s.presence = new List<Presence>();
        // s.presence.Add(new Presence { Date = DateTime.Now, IsPresent = true });
        System.Console.WriteLine($"Before Add: {students.Count}");
        this.students.Add(s);
        saveStudentJson();
        System.Console.WriteLine($"After Add: {students.Count}");
        return s.Id;
    }
    public bool UpdateStudent(int id, Student s)
    {
        if (isEmpty(s))
            return false;        
        Student currentStudent = students.FirstOrDefault( s => s.Id == id);
        if(currentStudent == null)
            return false;
        currentStudent.Name = s.Name;
        currentStudent.presence = s.presence;
        saveStudentJson();
        return true;
    }
    public bool UpdateTeacher(int id, Teacher t)
    {
        if (t == null || string.IsNullOrWhiteSpace(t.Name))
            return false;
        
        Teacher currentTeacher = teachers.FirstOrDefault( t => t.Id == id);
        if(currentTeacher == null)
            return false;
        
        currentTeacher.Name = t.Name;
        currentTeacher.Subject = t.Subject;
        saveTeacherJson();
        return true;
    }
    public bool DeleteTeacher(int id)
    {
        Teacher? currentTeacher = GetTeacher(id);
        System.Console.WriteLine(currentTeacher);
        if(currentTeacher ==null)
        return false;

        teachers.Remove(currentTeacher);
        saveTeacherJson();
        return true;
    }
    public bool DeleteStudent(int id)
    {
        Student currentStudent = GetStudent(id);
        System.Console.WriteLine(currentStudent);
        if(currentStudent ==null)
        return false;

        students.Remove(currentStudent);
        saveStudentJson();
        return true;
    }
}