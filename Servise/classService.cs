using MyProject.models;
using myProject.Controllers;
namespace myProject.Servise;
public static class ClassService
{
    private static List<Student> clas;
    static ClassService()
    {
        clas = new List<Student>{
            new Student { id = 100 , name = "gili kassel" },
            new Student { id = 101 , name = "chani levi" },
            new Student { id = 102 , name = "shoshi cohen" },
            new Student { id = 103 , name = "sari lev" },
            new Student { id = 104 , name = "chaya shapira" },
            new Student { id = 105 , name = "esti altman" },
        };
    }

    public static List<Student> Get(){
        return clas;
    }

    public static Student Get(int id){
        Student currentStudent = clas.FirstOrDefault(s => s.id == id);
        return currentStudent;
    }

    public static int Insert(Student s ){
        if (isEmpty(s)){
             return -1;
        }
       

        int maxId = clas.Max( s => s.id);
        s.id = maxId+1;
        clas.Add(s);

        return s.id;
    }

    public static bool UpDate(int id , Student s){
        if (isEmpty(s))
            return false;
        
        Student currentStudent = clas.FirstOrDefault( s => s.id == id);
        if(currentStudent == null)
            return false;
        
        currentStudent.name = s.name;
        // currentStudent.presence = s.presence;

        return true;
    }

    public static bool Delete( int id){
        Student currentStudent = clas.FirstOrDefault(s => s.id == id);
        if(currentStudent ==null)
        return false;

        clas.Remove(currentStudent);

        return true;
    }

      public static bool isEmpty(Student  s)
    {
        return s == null || string.IsNullOrWhiteSpace (s.name);
    }
}